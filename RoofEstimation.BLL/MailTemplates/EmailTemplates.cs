namespace RoofEstimation.BLL.MailTemplates;

public static class EmailTemplates
{
    public static readonly string WelcomeEmail = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"" />
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/>
  <title>Welcome Email</title>
</head>
<body style=""margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f5f7fa;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""padding: 40px 0;"">
    <tr>
      <td align=""center"">
        <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #ffffff; border-radius: 12px; padding: 40px;"">
          <tr>
            <td align=""center"" style=""padding-bottom: 20px;"">
              <!-- Logo SVG -->
              <svg width=""72"" height=""72"" viewBox=""0 0 72 72"" fill=""none"" xmlns=""http://www.w3.org/2000/svg"">
                <path d=""M34.2126 14.0805C35.2089 13.1157 36.7911 13.1157 37.7874 14.0805L62.8271 38.329C63.5768 39.055 64 40.054 64 41.0976V59.6506L56.7742 52.6531V40.7495C56.7742 40.04 56.199 39.4648 55.4895 39.4648H50.8331C50.1236 39.4648 49.5484 40.04 49.5484 40.7495V45.6556L37.7874 34.2663C36.7911 33.3015 35.2089 33.3015 34.2126 34.2663L8 59.6506V41.0976C8 40.054 8.42322 39.055 9.17292 38.329L34.2126 14.0805Z"" fill=""#1A56DB""/>
              </svg>
              <h2 style=""margin: 16px 0 0 0; font-size: 20px; font-weight: bold;"">RoofEst</h2>
              <p style=""font-size: 12px; color: #666;"">ESTIMATE YOUR OWN ROOF</p>
            </td>
          </tr>
          <tr>
            <td align=""center"" style=""padding: 20px 0;"">
              <h1 style=""font-size: 24px; margin: 0;"">Welcome on board!<br />Let’s get started.</h1>
            </td>
          </tr>
          <tr>
            <td style=""padding: 20px; color: #333; font-size: 14px; line-height: 1.6;"">
              <p>Hello {{ Name }},</p>
              <p>
                We’re excited to introduce new updates designed to enhance your experience and improve overall performance.
              </p>
              <p>
                Starting today, we’re rolling out streamlined terms and policies to ensure clarity and consistency across all our services. These updates will apply to any new subscriptions, renewals, and upgrades, with full adoption for existing accounts in 30 days.
              </p>
              <div style=""text-align: center; margin: 30px 0;"">
                <a href=""#"" style=""background-color: #1A56DB; color: #fff; padding: 12px 24px; text-decoration: none; border-radius: 6px; display: inline-block;"">
                  Button text
                </a>
              </div>
              <p>
                Our goal is to make it easier for you to navigate and manage your subscription. You can find detailed information and an FAQ on our website.
              </p>
              <p>Let us know if you have any questions!</p>
            </td>
          </tr>
          <tr>
            <td style=""font-size: 10px; color: #999; text-align: center; padding-top: 30px; border-top: 1px solid #eee;"">
              White River Roofing will furnish all materials and labor in order to reroof the roof of this property in a professional manner using standard practices and top quality material. Replacement of metal pipe flashings, paint new metal, and installation of new roof dormer vents per city code is free of charge!
            </td>
          </tr>
        </table>

        <!-- Footer -->
        <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""margin-top: 12px;"">
          <tr>
            <td style=""background-color: #1A56DB; color: #fff; text-align: center; padding: 10px 0; font-size: 12px; border-radius: 0 0 12px 12px;"">
              KEEPING YOU DRY SINCE 1992 · <strong>CA LIC #1007021</strong>
            </td>
          </tr>
        </table>

        <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""margin-top: 16px; color: #666; font-size: 12px; text-align: center;"">
          <tr>
            <td>
              <strong>RoofEst</strong><br />
              White River Roofing, Inc<br />
              1342 Ascote Ave<br />
              Sacramento, CA 95673<br />
              (916) 813-ROOF (7663)
            </td>
          </tr>
        </table>

      </td>
    </tr>
  </table>
</body>
</html>
";
    
    public static readonly string NewWelcomeEmail = @"
    <!DOCTYPE html>
    <html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">
    <head>
      <meta charset=""utf-8""> <!-- utf-8 works for most cases -->
      <meta name=""viewport"" content=""width=device-width""> <!-- Forcing initial-scale shouldn't be necessary -->
      <meta http-equiv=""X-UA-Compatible"" content=""IE=edge""> <!-- Use the latest (edge) version of IE rendering engine -->
      <meta name=""x-apple-disable-message-reformatting"">  <!-- Disable auto-scale in iOS 10 Mail entirely -->
      <title></title> <!-- The title tag shows in email notifications, like Android 4.4. -->

      <link href=""https://fonts.googleapis.com/css?family=Lato:300,400,700"" rel=""stylesheet"">

      <!-- CSS Reset : BEGIN -->
      <style>
          html, body {
              margin: 0 auto !important;
              padding: 0 !important;
              height: 100% !important;
              width: 100% !important;
              background: #f1f1f1;
          }
          * {
              -ms-text-size-adjust: 100%;
              -webkit-text-size-adjust: 100%;
          }
          div[style*=""margin: 16px 0""] {
              margin: 0 !important;
          }
          table, td {
              mso-table-lspace: 0pt !important;
              mso-table-rspace: 0pt !important;
          }
          table {
              border-spacing: 0 !important;
              border-collapse: collapse !important;
              table-layout: fixed !important;
              margin: 0 auto !important;
          }
          img {
              -ms-interpolation-mode:bicubic;
          }
          a {
              text-decoration: none;
          }
          *[x-apple-data-detectors], .unstyle-auto-detected-links *, .aBn {
              border-bottom: 0 !important;
              cursor: default !important;
              color: inherit !important;
              text-decoration: none !important;
              font-size: inherit !important;
              font-family: inherit !important;
              font-weight: inherit !important;
              line-height: inherit !important;
          }
          .a6S {
              display: none !important;
              opacity: 0.01 !important;
          }
          .im {
              color: inherit !important;
          }
          img.g-img + div {
              display: none !important;
          }
          @media only screen and (min-device-width: 320px) and (max-device-width: 374px) {
              u ~ div .email-container {
                  min-width: 320px !important;
              }
          }
          @media only screen and (min-device-width: 375px) and (max-device-width: 413px) {
              u ~ div .email-container {
                  min-width: 375px !important;
              }
          }
          @media only screen and (min-device-width: 414px) {
              u ~ div .email-container {
                  min-width: 414px !important;
              }
          }
      </style>
      <style>
          .primary { background: #30e3ca; }
          .bg_white { background: #ffffff; }
          .bg_light { background: #fafafa; }
          .bg_black { background: #000000; }
          .bg_dark { background: rgba(0,0,0,.8); }
          .email-section { padding:2.5em; }
          .btn { padding: 10px 15px; display: inline-block; }
          .btn.btn-primary { border-radius: 5px; background: #1A56DB; color: #ffffff; }
          h1,h2,h3,h4,h5,h6 { font-family: 'Lato', sans-serif; margin-top: 0; font-weight: 400; }
          body { font-family: 'Lato', sans-serif; font-weight: 400; font-size: 15px; line-height: 1.8; color: rgba(0,0,0,.4); }
          a { color: #30e3ca; }
          .logo h1 { margin: 0; }
          .logo h1 a { color: #30e3ca; font-size: 24px; font-weight: 700; font-family: 'Lato', sans-serif; }
          .hero { position: relative; z-index: 0; }
          .hero .text { color: #000; }
          .hero .text h2 { color: #000; font-size: 40px; margin-bottom: 0; font-weight: 400; line-height: 1.4; }
          .hero .text h3 { font-size: 24px; font-weight: 300; }
          .hero .text h2 span { font-weight: 600; color: #30e3ca; }
          .heading-section h2 { color: #000000; font-size: 28px; margin-top: 0; line-height: 1.4; font-weight: 400; }
          .footer { border-top: 1px solid rgba(0,0,0,.05); color: rgba(0,0,0,.5); }
          .footer .heading { color: #000; font-size: 20px; }
          .footer ul { margin: 0; padding: 0; }
          .footer ul li { list-style: none; margin-bottom: 10px; }
          .footer ul li a { color: rgba(0,0,0,1); }
      </style>
    </head>
    <body width=""100%"" style=""margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f1f1f1;"">
    <center style=""width: 100%; background-color: #f1f1f1;"">
      <div style=""max-width: 640px; margin: 0 auto; padding-top: 120px"" class=""email-container"">
        <table align=""center"" role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""margin: auto;"">
          <tr>
            <td valign=""middle"" class=""hero bg_white"" style=""padding-top: 48px; border-radius: 15px 15px 0px 0px;"">
              <img src=""https://api.roof-est.com/api/files/getLogo"" alt="""" style=""width: 170px; max-width: 600px; height: auto; margin: auto; display: block;"">         
            </td>
          </tr>
          <tr>
            <td valign=""middle"" class=""hero bg_white"" style=""padding-top: 48px"">
              <table>
                <tr>
                  <td>
                    <div class=""text"" style=""text-align: center;"">
                      <h2>Welcome on board!</h2>
                      <h2>Let’s get started.</h2>
                    </div>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td valign=""middle"" class=""hero bg_white"" style=""padding-top: 64px; padding-left: 120px; padding-right: 120px;"">
              <table>
                <tr>
                  <td>
                    <div class=""text"" style=""text-align: center;"">
                      <p>Hello {{Name}}</p>
                      <p>We’re excited to introduce new updates designed to enhance your experience and improve overall performance.</p>
                      <p>Starting today, we’re rolling out streamlined terms and policies to ensure clarity and consistency across all our services. These updates will apply to any new subscriptions, renewals, and upgrades, with full adoption for existing accounts in 30 days.</p>
                    </div>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td valign=""middle"" class=""hero bg_white"" style=""padding-top: 48px"">
              <table>
                <tr>
                  <td>
                    <div class=""text"" style=""text-align: center;"">
                      <a href=""#"" class=""btn btn-primary"" style=""padding-left: 24px; padding-right: 24px"">Button Text</a>
                    </div>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td valign=""middle"" class=""hero bg_white"" style=""padding-top: 48px; padding-left: 120px; padding-right: 120px;"">
              <table>
                <tr>
                  <td>
                    <div class=""text"" style=""text-align: center;"">
                      <p>Our goal is to make it easier for you to navigate and manage your subscription. You can find detailed information and an FAQ on our website.</p>
                      <p>Let us know if you have any questions!</p>
                    </div>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td valign=""top"" class=""bg_white"" style=""padding-top: 64px; padding-left: 120px; padding-right: 120px;"">
              <hr/>
            </td>
          </tr>
          <tr>
            <td valign=""middle"" class=""hero bg_white"" style=""padding-top: 64px; padding-left: 95px; padding-right: 95px;"">
              <table>
                <tr>
                  <td>
                    <div class=""text"" style=""text-align: center;"">
                      <p>White River Roofing will furnish all materials and labor in order to reroof the roof of this property in a professional manner using standard practices and top quality material. Replacement of metal pipe flashings, paint new metal, and installation of new roof dormer vents per city code is free of charge!</p>
                    </div>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td valign=""middle"" class=""hero bg_white"" style=""width: 100%; border-radius: 0px 0px 15px 15px; padding-top: 40px"">
              <table>
                <tr>
                  <td>
                    <div class=""text"" style=""padding-top: 17px; padding-bottom: 16px; text-align: center; width: 500px; background-color: #1A56DB; border-radius: 10px 10px 0px 0px; color: white; font-size: 12px;"">
                      KEEPING YOU DRY SINCE 1992 - <b>CA LIC #1007021</b>
                    </div>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </table>
      </div>
    </center>
    </body>
    </html>";
    }
