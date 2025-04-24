#!/bin/bash
project="morent-server/Morent.Infrastructure"
startupProject="morent-server/Morent.WebApi"

if [ -d ${project}/Migrations.db ]; then
  rm -rf "${project}/Migrations"
fi
if [ -e ${startupProject}/Morent.db ]; then
  mv ${startupProject}/Morent.db "${startupProject}/Morent_backup_$(date +%s).db"
fi
dotnet ef migrations add "InitialCreate" --project "${project}" --startup-project "${startupProject}"
dotnet ef database update --project ${startupProject}