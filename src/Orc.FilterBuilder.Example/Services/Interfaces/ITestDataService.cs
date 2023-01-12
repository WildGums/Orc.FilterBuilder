namespace FilterBuilder.Example.Services
{
    using System;
    using System.Collections.ObjectModel;
    using Models;

    public interface ITestDataService
    {
        ObservableCollection<TestEntity> GetTestItems();
        ObservableCollection<TestEntity> GenerateTestItems();
        TestEntity GenerateRandomEntity();
        DateTime GetRandomDateTime();
        string GetRandomString();
    }
}