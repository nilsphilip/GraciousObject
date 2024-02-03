using GlassBee.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GraciousObjectUtility.Tests
{
  [TestClass]
  public class GraciousObjectTest
  {
    const string ARBITRARY_VALUE = "MyValue";

    public dynamic Obj { get; set; }

    [TestInitialize]
    public void Setup()
    {
      Obj = new GraciousObject();
    }

    [TestMethod]
    public void Uninitialized_Property_IsFalse()
    {
      Assert.IsFalse(Obj.MyName);
    }

    [TestMethod]
    public void SetNull_Property_IsFalse()
    {
      Obj.xyz = null;
      Obj.abc = (bool?)null;

      Assert.IsFalse(Obj.xyz);
      Assert.IsFalse(Obj.abc);
    }

    [TestMethod]
    public void SetProperty_IsNotFalse()
    {
      Obj.MyValue = ARBITRARY_VALUE;

      Assert.IsTrue(!(Obj.MyValue is false));
    }

    [TestMethod]
    public void Property_IsCaseInsensitive()
    {
      Obj.MyName = ARBITRARY_VALUE;

      Assert.AreEqual(Obj.MyName, ARBITRARY_VALUE);
      Assert.AreEqual(Obj.myName, ARBITRARY_VALUE);
      Assert.AreEqual(Obj.myname, ARBITRARY_VALUE);
    }

    [TestMethod]
    public void Property_IsOverwriteable()
    {
      const string ARBITRARY_VALUE2 = "456";

      Obj.MyName = ARBITRARY_VALUE;
      Obj.MyName = ARBITRARY_VALUE2;

      Assert.AreEqual(Obj.MyName, ARBITRARY_VALUE2);
    }

    [TestMethod]
    public void Property_IsOverwriteable_DifferentType()
    {
      const string ARBITRARY_VALUE_TYPE1 = "123";
      const int ARBITRARY_VALUE_TYPE2 = 456;

      Obj.MyName = ARBITRARY_VALUE_TYPE1;
      Obj.MyName = ARBITRARY_VALUE_TYPE2;

      Assert.AreEqual(Obj.MyName, ARBITRARY_VALUE_TYPE2);
    }

    [TestMethod]
    public void Property_HasCorrectType()
    {
      Obj.MyValue = ARBITRARY_VALUE;

      if (Obj.MyValue is string text)
      {
        Assert.AreEqual(text, ARBITRARY_VALUE);
      }
      else
      {
        Assert.Fail();
      }
    }

    [TestMethod]
    public void Add_Property_Notifies()
    {
      var IsOnPropertyChanged = false;

      void OnPropertyChanged(object o, PropertyChangedEventArgs e)
      {
        Assert.IsNotNull(o);
        Assert.IsNotNull(e);
        Assert.AreEqual(Obj, o);
        Assert.AreEqual("notify", e.PropertyName);
        IsOnPropertyChanged = true;
      }

      ((INotifyPropertyChanged)Obj).PropertyChanged += OnPropertyChanged;

      Obj.notify = ARBITRARY_VALUE;

      Assert.IsTrue(IsOnPropertyChanged);
    }

    [TestMethod]
    public void Add_Property_Notifies_CaseSensitive()
    {
      void OnPropertyChanged(object o, PropertyChangedEventArgs e)
      {
        Assert.AreEqual("ToNotify", e.PropertyName);
      }

      ((INotifyPropertyChanged)Obj).PropertyChanged += OnPropertyChanged;

      Obj.ToNotify = ARBITRARY_VALUE;
    }

    [TestMethod]
    public async Task Property_ConcurrentAccessWorks()
    {
      const int INCREMENT_AMOUNT = 1000;

      Obj.MyValue1 = 0;
      Obj.MyValue2 = 0;

      Action<int> increment = new Action<int>(taskNumber =>
      {
        for (var i = 0; i < INCREMENT_AMOUNT; i++)
        {
          switch (taskNumber)
          {
            case 1:
              Obj.MyValue1 = Obj.MyValue1 + 1;
              break;
            case 2:
              Obj.MyValue2 = Obj.MyValue2 + 1;
              break;
          }
        }
      });

      var task1 = Task.Run(() => increment(1));
      var task2 = Task.Run(() => increment(2));

      await Task.WhenAll(task1, task2);

      Assert.AreEqual(INCREMENT_AMOUNT, Obj.MyValue1 as int?);
      Assert.AreEqual(INCREMENT_AMOUNT, Obj.MyValue2 as int?);
    }
  }
}