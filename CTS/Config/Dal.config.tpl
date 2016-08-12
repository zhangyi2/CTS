<dal name="FxPortalDB.config">
	<databaseSets>
		<databaseSet name="FxPortalDB_W" provider="mySqlProvider">
      <add name="FxPortalDB_W" databaseType="Master" sharding="" connectionString="FxPortalDB_W{$DBDataCenter}"/>
    </databaseSet>
	</databaseSets>
	
	<databaseProviders>
        <add name="sqlProvider" type="Arch.Data.DbEngine.Providers.SqlDatabaseProvider,Arch.Data"/>
        <!--如果只用Sql Server,请注释掉下面这行，否则可能有运行时错误-->
        <add name="mySqlProvider" type="Arch.Data.MySqlProvider.MySqlDatabaseProvider,Arch.Data.MySqlProvider"/>
	</databaseProviders>

	<logListeners>
		<add name="clog" type="Arch.Data.Common.Logging.Listeners.CentralLoggingListener,Arch.Data" level="Information" setting=""/>
	</logListeners>

	<metrics name="centrallogging"/>
</dal>