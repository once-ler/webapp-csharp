using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace webserver.contracts {
  public class BindableObjects<T> {
    // dummy abstract class
    public BindableObjects(T _obj) {
      this.obj = _obj;
    }
    public T obj { get; set; }
    public string toJson() {
      return JsonConvert.SerializeObject(obj, Formatting.None);
    }
  }
}
