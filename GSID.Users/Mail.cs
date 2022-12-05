using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace GSID.Users
{
    public class Mail
    {
        private string plantilla =$@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset = 'UTF-8' >
    < meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name = 'viewport' content='width=device-width, initial-scale=1.0'>
    <title>Document</title>
</head>
<body>
    <table border = '1' style='width: 100%;border: solid 1px ;' >
        <thead>
            <tr>
                <th colspan = '2' style='padding-bottom: .40em; padding-top: .50em;'>
                   <center><img src = 'https://d3egtvtajmesev.cloudfront.net/wp-content/uploads/2022/02/08224928/logo-300-mercasid.png' width='20%' /></center> 
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style = 'width: 60% ;' ></ td >
                < td style='color:#6a6666bf; text-align:right;padding-right: 5%;'>06/30/2022</td>
            </tr>
            <tr>
                <td colspan = '2' style='background-color: #25b7d3 ;color:white;'><center><h3>Titulo</h3></center></td>
            </tr>
            <tr>
                <td colspan = '2' style='padding: 10px ;' ><center><p>But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness.No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful.Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure</p></center></td>
            </tr>
            <!-- <tr>
                <td colspan = '2' >
                    < table border= '1' style= 'width: 100%;' >
                        < tbody  style= 'width: 100%;' >
                            < tr style= 'width: 100%;' >
                                < td style= 'width: 30%;' >
                                    < img style= 'width:25%' src= 'https://d3egtvtajmesev.cloudfront.net/wp-content/uploads/2022/02/08224928/logo-300-mercasid.png' />
                                </ td >
                                < td >
                                        texto
                                </ td >
                            </ tr >
                        </ tbody >


                    </ table >
                </ td >
            </ tr > -->

            < tr >
                < td colspan= '2' >
                    < table  style= 'width: 100%;' >
                        < tbody  style= 'width: 100%;' >
                            < tr style= 'width: 100%;' >
                                < td style= 'width: 30%;' >
                                  < center > < img  style= 'width: 15%;' src= 'https://i.postimg.cc/4xgFS2kc/informacion.png' /></ center >
                                </ td >
                            </ tr >

                            < tr style= 'width: 100%;' >


                                < td >
                                     < center > +item1 + item2 + item3 + item4 </ center >
                                </ td >
                            </ tr >
                        </ tbody >


                    </ table >
                </ td >
            </ tr >


        </ tbody >

    </ table >
</ body >
</ html >






";

        public static void sendMail(string to,string toLabel ,string subject, string body ) {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("test.eudy.arias@outlook.es", "Correo de prueba");
            mail.To.Add(new MailAddress(to, toLabel));
            //mail.To.Add(new MailAddress("test.eudy.arias@gmail.com", "Alberto"));
            mail.Subject = subject;
            mail.Body = body;
            mail.Priority = MailPriority.Normal;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.Host = "smtp-mail.outlook.com"; //"smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;

            System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential();
            //networkCredential.UserName = "test.eudy.arias@gmail.com";
            networkCredential.UserName = "test.eudy.arias@outlook.es";
            networkCredential.Password = "emmasofia1990";

            smtp.Credentials = networkCredential;
            smtp.Send(mail);
        } 
    }
}
