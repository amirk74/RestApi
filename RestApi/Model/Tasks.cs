using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RestApi.Model
{
    public static class Tasks
    {

        public static MyEntity AddEntity(MyEntity input)
        {
            CloudTable table = Tasks.CreateIfNotExists();
            var entity = new MyEntity() { PartitionKey = "emailattachments", RowKey = Guid.NewGuid().ToString(), Subject = input.Subject, Body = input.Body, BlobURLs = input.BlobURLs, Type = input.Type };
            TableOperation insert = TableOperation.Insert(entity);
            table.ExecuteAsync(insert);
            return entity;
        }

        public static MyEntity UpdateEntity(MyEntity input, string id)
        {
            MyEntity entity = Tasks.RetrieveEntity(id);
            entity.Subject = input.Subject;
            entity.BlobURLs = input.BlobURLs;
            entity.Body = input.Body;
            entity.Type = input.Type;
            UpdateEntity(entity);
            return entity;
        }


        static void UpdateEntity(MyEntity entity)
        {
            CloudTable table = CreateIfNotExists();
            TableOperation update = TableOperation.Replace(entity);
            table.ExecuteAsync(update);
        }

        public static MyEntity RetrieveEntity(string rowKey)
        {
            CloudTable table = CreateIfNotExists();
            TableOperation tableOperation = TableOperation.Retrieve<MyEntity>("emailattachments", rowKey);
            TableResult tableResult =  table.ExecuteAsync(tableOperation).Result;
            return tableResult.Result as MyEntity;
        }


        public static List<MyEntity> RetrieveAllEntities()
        {
            TableContinuationToken continuationToken = null;
            CloudTable table = CreateIfNotExists();
            TableQuery<MyEntity> query = new TableQuery<MyEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "emailattachments"));
            return table.ExecuteQuerySegmentedAsync(query, continuationToken).Result.Results;
        }

        public static void DeleteEntity(string id)
        {
            MyEntity entity = Tasks.RetrieveEntity(id);
            CloudTable table = Tasks.CreateIfNotExists();
            TableOperation delete = TableOperation.Delete(entity);
            table.ExecuteAsync(delete);
        }
        static CloudTable CreateIfNotExists()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(Environment.GetEnvironmentVariable("AzureWebJobsTableName"));
            table.CreateIfNotExistsAsync();
            return table;
        }


    }
}
