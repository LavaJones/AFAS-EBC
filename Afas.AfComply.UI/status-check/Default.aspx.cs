using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

using log4net;
using System.Diagnostics;

public partial class status_check_Default : PageBase
{
    private ILog MainLogger = LogManager.GetLogger(typeof(status_check_Default));

    public status_check_Default() : base()
    {        
        this.Log = LogManager.GetLogger(String.Format("StatusCheck.{0}", typeof(status_check_Default).FullName));
    }

    protected override void OnInit(EventArgs eventArgs)
    {
     
        base.OnInit(eventArgs); 
     
        if (User.Identity.IsAuthenticated)
        {
            ViewStateUserKey = Session.SessionID;
        }
    
    } 
    
    protected void Page_Load(Object sender, EventArgs e)
    {

        Stopwatch watch = new Stopwatch();
        watch.Start();

        String accessToken = Request.QueryString["access_token"];

        if (accessToken == null)
        {

            this.Log.Warn(String.Format("Received unauthorized request. AccessToken is missing."));

            throw new HttpException(404, "Resource not found.");

        }

        if (accessToken.Length == 0)
        {

            this.Log.Warn(String.Format("Received unauthorized request. AccessToken is empty."));

            throw new HttpException(404, "Resource not found.");

        }

        Guid accessGuid = Guid.Empty;

        if (Guid.TryParse(accessToken, out accessGuid) == false)
        {

            this.Log.Warn(String.Format("Received unauthorized request. AccessToken is not a valid GUID."));

            throw new HttpException(404, "Resource not found.");

        }

        if (status_check_Default.AccessFailureGuid.Equals(accessGuid))
        {

            this.Log.Warn(String.Format("Short circuit, placebo testing GUID detected."));

            this.Log.Error("Short circuit, placebo testing GUID detected.", new InvalidOperationException());

            throw new HttpException(500, "Placebo testing GUID detected.");

        }

        if (status_check_Default.AccessGuid.Equals(accessGuid) == false)
        {

            this.Log.Warn(String.Format("Received unauthorized request with accessGuid: {0}.", accessGuid));

            throw new HttpException(404, "Resource not found.");

        }

        Stopwatch filewatch = new Stopwatch();
        filewatch.Start();
        Stopwatch file1watch = new Stopwatch();
        file1watch.Start();

        String filename = Guid.NewGuid().ToString().Replace("-", String.Empty);
        if (this.Log.IsDebugEnabled) 
        { 
            this.Log.Debug(String.Format("Status Check filename: {0}", filename)); 
        }

        String folder = Server.MapPath(Archive.ArchiveFolder);
        if (this.Log.IsDebugEnabled) 
        {
            this.Log.Debug(String.Format("Archive Folder path: {0}", folder)); 
        }

        String fullyQualifedFilename = String.Format("{0}\\{1}.status-check.txt", folder, filename);
        if (this.Log.IsDebugEnabled) 
        { 
            this.Log.Debug(String.Format("Status Check fully qualified filename: {0}", fullyQualifedFilename)); 
        }
        file1watch.Stop();

        Stopwatch file2watch = new Stopwatch();
        file2watch.Start();
        Guid fileCheckGuid = Guid.NewGuid();

        try
        {

            using (StreamWriter exportFileStream = new StreamWriter(fullyQualifedFilename))
            {
                exportFileStream.WriteLine(fileCheckGuid.ToString());
            }

        }
        catch (Exception exception)
        {

            this.Log.Error("Exceptions during file writing.", exception);

            throw new HttpException(500, "File Write Access.");

        }
        file2watch.Stop();

        Stopwatch file3watch = new Stopwatch();
        file3watch.Start();
        try
        {

            String readGuid = String.Empty;

            using (StreamReader importFileStream = new StreamReader(fullyQualifedFilename))
            {

                while ((readGuid = importFileStream.ReadLine()) != null)
                {

                    if (Guid.Parse(readGuid).Equals(fileCheckGuid) == false)
                    {
                        throw new IOException("File does not contain the information that was written.");
                    }

                }

            }

        }
        catch (Exception exception)
        {

            this.Log.Error("Exceptions during file reading.", exception);

            throw new HttpException(500, "File Read Access.");

        }
        file3watch.Stop();

        Stopwatch file4watch = new Stopwatch();
        file4watch.Start();
        try
        {
            
            File.Delete(fullyQualifedFilename);

            if (File.Exists(fullyQualifedFilename))
            {
                throw new IOException("Tried to delete a file but it still exists on the file system.");
            }

        }
        catch (Exception exception)
        {

            this.Log.Error("Exceptions during file removal.", exception);

            throw new HttpException(500, "File Removal Access.");

        }
        file4watch.Stop();


        filewatch.Stop();

        Stopwatch dbwatch = new Stopwatch();
        dbwatch.Start();

        UserFactory userFactory = new UserFactory();
        
        try
        {

            List<User> users = UserController.getDistrictUsers(1);
            if(users.Count <= 0)
            {
                throw new IOException("Database read failed.");
            }

        }
        catch (Exception exception)
        {

            this.Log.Error("Exceptions during database access.", exception);

            throw new HttpException(500, "Database Access.");

        }

        try
        {


            if (false == userFactory.updateFloatingUser(1, SystemSettings.UserDbId))
            {
                throw new IOException("Database write failed.");
            }

        }
        catch (IOException exception)
        {

            this.Log.Error("Exceptions during database access.", exception);

            throw new HttpException(500, "Database Access.");

        }
        catch (Exception exception)
        {
            
            this.Log.Warn("Exceptions during database access.", exception);

        }

        dbwatch.Stop();
        watch.Stop();

        if (watch.ElapsedMilliseconds > Feature.ShortTimeStatusCheck)
        {
            if (watch.ElapsedMilliseconds > Feature.LongTimeStatusCheck)
            {
                MainLogger.Error("StatusCheck page was very slow. Total:[" + watch.ElapsedMilliseconds+"]ms, File Access:["+ filewatch.ElapsedMilliseconds+ "]ms, DB Access:["+dbwatch.ElapsedMilliseconds+"]ms, File check setup :["+ file1watch.ElapsedMilliseconds + "]ms, File create/write check :["+ file2watch.ElapsedMilliseconds+"]ms, File read check :["+ file3watch .ElapsedMilliseconds+ "]ms, File Delete Check :["+ file4watch.ElapsedMilliseconds+ "]ms");
            }
            else
            {
                MainLogger.Warn("StatusCheck page was  slow. Total:[" + watch.ElapsedMilliseconds + "]ms, File Access:[" + filewatch.ElapsedMilliseconds + "]ms, DB Access:[" + dbwatch.ElapsedMilliseconds + "]ms, File check setup :[" + file1watch.ElapsedMilliseconds + "]ms, File create/write check :[" + file2watch.ElapsedMilliseconds + "]ms, File read check :[" + file3watch.ElapsedMilliseconds + "]ms, File Delete Check :[" + file4watch.ElapsedMilliseconds + "]ms");
            }
        }


    }

    public static readonly Guid AccessGuid = Guid.Parse("E14CA133-4238-4DC1-AB85-55B51DBB41FA");

    public static readonly Guid AccessFailureGuid = Guid.Parse("DEADBEEF-4238-4DC1-AB85-55B51DBB41FA");

    protected ILog Log { get; private set; }

}
