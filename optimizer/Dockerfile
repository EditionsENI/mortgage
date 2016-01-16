FROM aspnetbase:1.0.0-beta6
ADD /src /app
WORKDIR /app
RUN ["dnu", "restore"]
EXPOSE 5004
ENTRYPOINT sleep 10000000 | dnx . kestrel # https://github.com/docker/compose/issues/1409
