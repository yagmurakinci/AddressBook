﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookBL.EmailSenderBusiness
{
    public interface IEmailSenderManager
    {
        bool SendEmail(EmailMessage message);
        Task SendEmailAsync(EmailMessage message);
    }
}
