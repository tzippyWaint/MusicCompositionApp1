using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using Models;
using MusicCompositionBL;



namespace MusicCompositionBL
{
    public class EmailAndPayPal
    {
        //פעולה בשביל התשלום##
        public bool PayOfAppearance(int cardNum,int threeLetters,DateTime effect,int sumOfPay)
        {
            return false;
        }
        //שליחת הודעת אימייל לנגנים(לזמן מהוספת ומחיקת הופעה)
        public static void SendMailToPlayers(Appearances appearance, List<string> accountments,string subject,int status)
        {
            try
            {
                classes.PlayersBL pbl = new classes.PlayersBL();
                string pelConductor = pbl.listOfAllPlayers.Find(p => p.codeP == appearance.codeConductor).pel;
                string nameConductor = pbl.listOfAllPlayers.Find(p => p.codeP == appearance.codeConductor).fullNameP;

                string email = "tirosh1979@gmail.com";
                string password = "pnini1979";
                /*
                LinkedResource inline = new LinkedResource(DTO.StartPoint.Liraz + "DAL\\Files\\icon.jpg", MediaTypeNames.Image.Jpeg);
                inline.ContentId = Guid.NewGuid().ToString();
                avHtml.LinkedResources.Add(inline);
                */
                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);


                msg.From = new MailAddress(email);
                //foreach (var item in accountments)
                //{
                //    msg.To.Add(new MailAddress(item));
                //}
                msg.To.Add(new MailAddress("tirosh1979@gmail.com"));
                string sInsert = "נתוספה ליומן אירועיך הופעה חדשה";
                if(status==1)
                    sInsert = "הודעה בדבר ביטול הופעה";

                msg.Subject = subject+" " + appearance.dateA;
                //LinkedResource res = new LinkedResource(DTO.StartPoint.Liraz + "DAL\\Files\\icon.png");
                //res.ContentId = Guid.NewGuid().ToString();
                #region buildHtmlMessageBody
                string htmlBodyString = string.Format(
                      @"
                       <div style='  direction: rtl;
                                     background - color: #aff1f1;
                                     font - family: Amerald;
                                     font - size:medium; '>
                              < div style='text-align:center;color: #193f3f;'>
                               <h1>שלום!</h1>
                               <h3>{5}</h3>
                           </div>
                           <div style='  position: relative;
                                         padding: 0.75rem 1.25rem;
                                         margin-bottom: 1rem;
                                         margin-left: 7%;
                                         margin-right: 7%;
                                         color: #313131;
                                         width: 75%;
                                         '>
                               <label> שם מנצח: {0}</label>
                               <br />
                               <label> פלאפון מנצח: {6}</label>
                               <br />
                               <label> תאריך הופעה: {1}</label>
                               <br />
                               <label> כתובת: {2}</label>
                               <br />
                               <label> שעת התחלה: {3}</label>
                               <br />
                               <label> שעת סיום: {4}</label>
                               <br />
                               <br />
                           </div>"

                         , nameConductor, appearance.dateA, appearance.addresPlace, appearance.startHour, appearance.endHour, sInsert,pelConductor);





                #endregion
                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBodyString, null, MediaTypeNames.Text.Html);
                //alternateView.LinkedResources.Add(res);
                msg.AlternateViews.Add(alternateView);
                msg.IsBodyHtml = true;
                 //msg.Attachments.Add(new Attachment(doctor.pictureDiploma));
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {


                throw ex;
            }
        }

    }
}
