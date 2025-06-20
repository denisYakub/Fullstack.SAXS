﻿using Fullstack.SAXS.Persistence.Contracts;

namespace Fullstack.SAXS.Persistence.Tests.Mocks
{
    internal class StringServiceMock : IStringService
    {
        public string GetCsvFolder()
        {
            var folderPath =
                Path
                .Combine(
                    AppContext.BaseDirectory,
                    "FileServiceTests"
                );

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return folderPath;
        }

        public string GetGraphUriPath()
        {
            throw new NotImplementedException();
        }
    }
}
