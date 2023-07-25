FROM mcr.microsoft.com/dotnet/sdk:7.0

COPY . /app

ENV DYLD_LIBRARY_PATH=/usr/bin/openssl

WORKDIR /app/Juspay-Test


