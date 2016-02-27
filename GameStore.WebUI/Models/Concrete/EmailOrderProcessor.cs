using GameStore.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.WebUI.Models.Concrete
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using(var smtpClient = new SmtpClient())
            {
                var s = Startup.Configuration["EmailSettings:ServerName"];
                smtpClient.Host = Startup.Configuration["EmailSettings:ServerName"];
                smtpClient.Port = int.Parse(Startup.Configuration["EmailSettings:ServerPort"]);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = bool.Parse(Startup.Configuration["EmailSettings:UseSsl"]);                
                smtpClient.Credentials = new NetworkCredential(
                    userName: Startup.Configuration["EmailSettings:UserName"],
                    password: Startup.Configuration["EmailSettings:Password"]
                    );
                if (bool.Parse(Startup.Configuration["EmailSettings:WriteAsFile"]))
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = Startup.Configuration["EmailSettings:FileLocation"];
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder()
                    .AppendLine("Новый заказ обработан")
                    .AppendLine("---")
                    .AppendLine("Товары");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Game.Price * line.Quantity;
                    body.AppendFormat("{0} X {1} (итого {2:c})", line.Quantity, line.Game.Name, subtotal);
                }
                body.AppendFormat("Общая стоимость: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Доставка")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2)
                    .AppendLine(shippingDetails.Line3)
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.Country)
                    .AppendLine("---")
                    .AppendFormat("Подарочная упаковка: {0}", shippingDetails.GiftWrap ? "Да" : "Нет");
                MailMessage mailMessage = new MailMessage(
                    from: Startup.Configuration["EmailSettings:MailFromAddress"],
                    to: Startup.Configuration["EmailSettings:MailToAddress"],
                    subject: "Новый заказ отправлен",
                    body: body.ToString()
                    );
                if (bool.Parse(Startup.Configuration["EmailSettings:WriteAsFile"]))
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}
