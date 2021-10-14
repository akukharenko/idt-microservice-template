dotnet sonarscanner begin /k:"boss-service-sample" /d:sonar.login="b35ae80b370fdc59bd701a9738a864a1813cf973" /d:sonar.host.url="http://localhost:9000"
dotnet build IDT.Boss.ServiceName.sln
dotnet sonarscanner end /d:sonar.login="b35ae80b370fdc59bd701a9738a864a1813cf973"
