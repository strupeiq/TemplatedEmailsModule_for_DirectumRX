using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using study.Study1.OfficialDocument;

namespace study.Study1.Server
{
  partial class OfficialDocumentFunctions
  {
    [Public, Remote]
    public virtual void SendEmail(string body, string email)
    {
      var message = Mail.CreateMailMessage();
      message.IsBodyHtml = true;
      message.Subject = _obj.Subject;
      message.Body = body;
      if (_obj.LastVersion != null)
        message.AddAttachment(_obj.LastVersion);
      message.To.Add(email);
      Mail.Send(message);
    }

  }
}