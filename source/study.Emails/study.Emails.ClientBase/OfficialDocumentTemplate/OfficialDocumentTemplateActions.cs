using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using study.Emails.OfficialDocumentTemplate;

namespace study.Emails.Client
{
  partial class OfficialDocumentTemplateActions
  {
    public virtual void CreateNewTemplate(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      
    }

    public virtual bool CanCreateNewTemplate(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}