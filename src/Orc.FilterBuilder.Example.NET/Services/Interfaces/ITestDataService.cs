// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITestDataService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


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