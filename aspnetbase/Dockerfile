FROM microsoft/aspnet:1.0.0-rc1-update1
ADD /src /app
WORKDIR /app
RUN ["dnu", "restore"]
ENV LASTREFRESH 2015-06-02
