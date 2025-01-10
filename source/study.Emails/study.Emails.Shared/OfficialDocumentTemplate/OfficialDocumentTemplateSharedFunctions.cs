using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using study.Emails.OfficialDocumentTemplate;

namespace study.Emails.Shared
{ 
  partial class OfficialDocumentTemplateFunctions
  {
    [Public]
    public virtual string GenerateTemplate(Sungero.Docflow.IOfficialDocument document){
      var result = _obj.TemplateBody;
      var vars = GetVars(document);
      foreach (var key in vars.Keys)
        result = result.Replace("{{" + key + "}}", vars[key]);
      return result;
    }
    
    public virtual System.Collections.Generic.Dictionary<string, string> GetVars(Sungero.Docflow.IOfficialDocument document)
    {
      var properties = new Dictionary<string, string>();
      var type = document.GetType();
      foreach (var property in type.GetProperties())
      {
        var propertyValue = property.GetValue(document)?.ToString() ?? "Пусто";
        properties[property.Name] = propertyValue;
      }
      return properties;
    }
  }
}