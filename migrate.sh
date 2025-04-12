#!/bin/bash
project="morent-server/Morent.Infrastructure"
startupProject="morent-server/Morent.WebApi"

rm -rf "${project}/Migrations"
mv ${startupProject}/Morent.db "${startupProject}/Morent_backup_$(date +%s).db"
dotnet ef migrations add "InitialCreate" --project "${project}" --startup-project "${startupProject}"
dotnet ef database update --project ${startupProject}