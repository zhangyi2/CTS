<?xml version="1.0" encoding="utf-8"?>
<!--
  For more information on how to configure your ASP.NET application, please visit
  http://go.microsoft.com/fwlink/?LinkId=169433
  -->
<configuration>
  <configSections>
    <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
    <section name="dal" type="Arch.Data.DbEngine.Configuration.DbEngineConfigurationSection, Arch.Data" />
    <section name="databaseMappings" type="Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DbMappingsSection, Microsoft.Practices.EnterpriseLibrary.Data"/>
    <section name="loggingProvider" type="Arch.CFramework.Provider.Configuration.LoggingProviderSection, Arch.CFX"/>
  </configSections>

  <appSettings configSource="Config\\AppSetting.config" />
  <dal configSource="Config\\Dal.config" />
  <databaseMappings configSource="Config\DatabaseMap.config" />
  <loggingProvider defaultProvider ="CLoggingProvider">
    <providers>
      <add name="CLoggingProvider" type="Arch.CFramework.CLoggingAdapter.CLoggingProvider, Arch.CFX.CLoggingAdapter" />
    </providers>
  </loggingProvider>

  <system.web>
    <compilation debug="{$CompilationDebug}" targetFramework="4.0" />
    <profile defaultProvider="DefaultProfileProvider">
      <providers>
        <add name="DefaultProfileProvider" type="System.Web.Providers.DefaultProfileProvider, System.Web.Providers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" connectionStringName="DefaultConnection" applicationName="/" />
      </providers>
    </profile>
    <membership defaultProvider="DefaultMembershipProvider">
      <providers>
        <add name="DefaultMembershipProvider" type="System.Web.Providers.DefaultMembershipProvider, System.Web.Providers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" connectionStringName="DefaultConnection" enablePasswordRetrieval="false" enablePasswordReset="true" requiresQuestionAndAnswer="false" requiresUniqueEmail="false" maxInvalidPasswordAttempts="5" minRequiredPasswordLength="6" minRequiredNonalphanumericCharacters="0" passwordAttemptWindow="10" applicationName="/" />
      </providers>
    </membership>
    <roleManager defaultProvider="DefaultRoleProvider">
      <providers>
        <add name="DefaultRoleProvider" type="System.Web.Providers.DefaultRoleProvider, System.Web.Providers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" connectionStringName="DefaultConnection" applicationName="/" />
      </providers>
    </roleManager>
    <!--
            If you are deploying to a cloud environment that has multiple web server instances,
            you should change session state mode from "InProc" to "Custom". In addition,
            change the connection string named "DefaultConnection" to connect to an instance
            of SQL Server (including SQL Azure and SQL  Compact) instead of to SQL Server Express.
      -->
    <webServices>
      <protocols>
        <add name="HttpGet"/>
        <add name="HttpPost"/>
      </protocols>
    </webServices>
  </system.web>
  <system.webServer>

    <defaultDocument>
      <files>

        <add value="Index.aspx" />
      </files>
    </defaultDocument>
    <validation validateIntegratedModeConfiguration="false" />

  </system.webServer>

  <system.net>
    <settings>
      <httpWebRequest useUnsafeHeaderParsing="true" />
    </settings>
  </system.net>
  <system.web.extensions>
    <scripting>
      <webServices>
        <jsonSerialization maxJsonLength="10000000"/>
      </webServices>
    </scripting>
  </system.web.extensions>
</configuration>