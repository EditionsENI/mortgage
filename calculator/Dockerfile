FROM mattovotech/dotnet2.0-sdk-mono-build AS builder
WORKDIR /app
ADD /src/calculator.csproj /app
RUN dotnet restore
ADD /src /app
RUN dotnet publish -o output

FROM stacklineorg/dotnet2.0-runtime-rocksdb
COPY --from=builder /app/output /app
WORKDIR /app
ENV URLOptimizerJobs=http://optimizer/api/Jobs/
ENTRYPOINT ["dotnet", "calculator.dll"]
