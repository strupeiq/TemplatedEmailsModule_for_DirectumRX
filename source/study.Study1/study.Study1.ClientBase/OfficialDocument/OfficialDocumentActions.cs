using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using study.Study1.OfficialDocument;

namespace study.Study1.Client
{
  partial class OfficialDocumentActions
  {
    public virtual void SendByMailWithTemplatestudy(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var currentDocument = Sungero.Docflow.OutgoingDocumentBases.As(_obj); //Кастуем к исходящему письму для вытягивания поля Addresse
      var memo = Sungero.Docflow.Memos.As(_obj); //Аналогично для служебной записки
      if (currentDocument == null && memo == null)
      {
        e.AddError("Можно отправить только исходящее письмо или служебную записку.");
        return;
      }
      
      if (currentDocument?.Addressee?.Email == null && memo?.Addressee?.Email == null)
        Dialogs.ShowMessage("Не указан адресат или у адресата отсутствует почта!",
                             "Ошибка при попытке создания письма из шаблона.",
                             MessageType.Error, "Ошибка");
      else
      {
        if (_obj.LastVersion == null)
          Dialogs.ShowMessage("Письмо будет отправлено без вложенного документа, т.к. у заполняемого документа отсутствует версия!",
                             "Сохраните версию и повторите попытку отправки, если Вам это необходимо.",
                            MessageType.Warning, "Предупреждение");
        var dialog = Dialogs.CreateInputDialog("Выберите шаблон");
        var allTemplates = study.Emails.OfficialDocumentTemplates.GetAll().Where(x => x.DocumentType == _obj.DocumentKind).ToArray(); //Создаем список шаблонов, доступных для заполняемого документа
        var templates = dialog.AddSelect("Шаблон", true, Emails.OfficialDocumentTemplates.Null).From(allTemplates);
      
        if (dialog.Show() == DialogButtons.Ok)
        {
          var emailBody = study.Emails.PublicFunctions.OfficialDocumentTemplate.GenerateTemplate(templates.Value, _obj); //Генерируем шаблон из заполненных в документе полей
          var sendDialog = Dialogs.CreateTaskDialog("Вы хотите отправить данное письмо?",
                                                   emailBody,
                                                  MessageType.Question,
                                                 "Предпросмотр шаблона");
          sendDialog.Buttons.AddOk();
          sendDialog.Buttons.AddCancel();
          if (sendDialog.Show() == DialogButtons.Ok)
          {
            if (memo == null)
              Functions.OfficialDocument.Remote.SendEmail(_obj, emailBody, currentDocument.Addressee.Email);
            else
              Functions.OfficialDocument.Remote.SendEmail(_obj, emailBody, memo.Addressee.Email);
          }
        }
        
      }
    }
  
      

    public virtual bool CanSendByMailWithTemplatestudy(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      var currentDocument = Sungero.Docflow.OutgoingDocumentBases.As(_obj);
      var memo = Sungero.Docflow.Memos.As(_obj);
      if (_obj.DocumentKind == memo?.DocumentKind || _obj.DocumentKind == currentDocument.DocumentKind)
        return true;
      return false;
    }
  }
}


