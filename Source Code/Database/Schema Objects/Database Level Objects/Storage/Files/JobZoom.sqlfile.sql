ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [JobZoom], FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

