# Afc Framework Presentation Web Mvc

## What has been added to your project
You will notice eight additional files in your project.

 * dependencies.config
	This is an empty Castle Windsor dependencies configuration file. Additional properties or components can be defined here. Any components that may change during runtime need to be defined here. Visit the Castle site to read up on the configuration format. 
	http://docs.castleproject.org/Windsor.XML-Registration-Reference.ashx

	Components that may need to be defined in the xml configuration file are those that you have not written but that you depend upon.
 
 * log4net.config
	This is a default log4net configuration that you are free to modify. Examples of configurations can be found online:

	http://logging.apache.org/log4net/release/config-examples.html

	There is a NAS logging location that is used when deploying to development, qa, and production. You need to use the FileAppender and allow the deployment to assign the log file location as needed. Contact the deployment team for information regarding this NAS logging folder.

 * App_Start\ContainerActivator.cs
	This class starts the dependency injection container instance and begins the component registration process via Winsor installers (see below). You shouldn't need to touch this file except under very rare cases.

 * Installers\ControllersInstaller.cs
	This is a Windsor installer that sets up the dependency injection of your MVC controllers. Any class that implements IController is registered automatically by this class. You should not need to modify it.
 
 * Installers\ServiceInstaller.cs
	This is a Windsor installer that registers any classes that inherit from Afc.Core.Application.BaseService. If you need any of your implementations of a BaseService just require the interface of your service in a constructor.

 * Installers\RepositoryInstaller.cs
	This is a Windsor installer that registers any classes that implement Afc.Core.Domain.IEntityRepository<> or Afc.Core.Domain.IRepository<> . As well as your EF DbContexts that implements Afc.Core.Domain.IDbContext.

 * Installers\EncryptedParametersInstaller.cs
	This is a Windsor installer that registers classes for use with encrypted query string parameters. To use encrypted query string parameters you need to specify the appropriate constraint on the appropriate routes as below:

		/* Within your route config */
            var encryptedQueryStringConstraint = DependencyResolver.Current.GetService<EncryptedParametersConstraint>();

            routes.MapRoute(
                name: "Some-Route-Name",
                url: "Some-Url",
                defaults: new { controller = "Home", action = "Example" },
                constraints: new { stuff = encryptedQueryStringConstraint }
            );
	
	Your Views/web.config file has been updated to include "Afc.Framework.Presentation.Web" as a namespace for all views to use:
	...
	<pages pageBaseType="System.Web.Mvc.WebViewPage">
		<namespaces>
			...
			<add namespace="Afc.Framework.Presentation.Web"/>
		</namespaces>
	</pages>
	...

	That gives all of your views the ability to use the ActionEncrypted Url helper in your views:
		<a href="@Url.ActionEncrypted("ActionName", "ControllerName", new { someparameter = "the value" } )">Link Text</a>


 * Plubming\WindsorControllerFactory.cs
	This is the MVC controller factory that uses Castle Windsor to build the controllers, services, repositories, etc. There is no reason to edit this class.

If you have additional items to register for use with dependency injection create an additional Windsor installer class and register the components from there. Visit the Castle site to read up on Windsor installers and component registration. 

http://docs.castleproject.org/Windsor.Fluent-Registration-API.ashx

