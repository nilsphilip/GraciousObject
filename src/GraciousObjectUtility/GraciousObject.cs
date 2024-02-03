using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace GlassBee.Utilities
{
  public class GraciousObject : DynamicObject, INotifyPropertyChanged
  {
    public GraciousObject() : base()
    {
      Properties = new ConcurrentDictionary<string, object>();
    }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    public IDictionary<string, object> Properties { get; }
    public int Count => Properties.Count;

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      var name = binder.Name.ToLower();

      if (Properties.TryGetValue(name, out result) == false)
      {
        result = false;
      }

      return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
      var name = binder.Name.ToLower();
      if (Properties.TryGetValue(name, out var oldValue)
        && oldValue == value)
      {
        return true;
      }
      else if (!(value is null))
      {
        Properties[name] = value;
        RaisePropertyChanged(binder.Name);
      }

      return true;
    }

    public void RaisePropertyChanged(string propertyName)
      => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}
