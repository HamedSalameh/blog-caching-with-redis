using cache_example.webapi.Cache;
using NSubstitute;
using StackExchange.Redis;

namespace cache_example.tests
{

    public class Tests
    {
        [Test]
        public async Task StringSetAsync_Should_Set_Value_In_Redis()
        {
            // Arrange
            var redisDbProvider = Substitute.For<IRedisDbProvider>();
            var cacheHandler = new RedisCacheHandler(redisDbProvider);
            var key = "testKey";
            var value = "testValue";

            // Act
            await cacheHandler.StringSetAsync(key, value);

            // Assert
            await redisDbProvider.database.Received(1).StringSetAsync(key, value, null);
        }

        [Test]
        public async Task StringExistsAsync_Should_Check_If_Key_Exists_In_Redis()
        {
            // Arrange
            var redisDbProvider = Substitute.For<IRedisDbProvider>();
            var cacheHandler = new RedisCacheHandler(redisDbProvider);
            var key = "testKey";

            // Act
            await cacheHandler.StringExistsAsync(key);

            // Assert
            await redisDbProvider.database.Received(1).KeyExistsAsync(key);
        }

        [Test]
        public async Task StringGetAsync_Should_Get_Value_From_Redis()
        {
            // Arrange
            var redisDbProvider = Substitute.For<IRedisDbProvider>();
            var cacheHandler = new RedisCacheHandler(redisDbProvider);
            var key = "testKey";

            // Act
            await cacheHandler.StringGetAsync(key);

            // Assert
            await redisDbProvider.database.Received(1).StringGetAsync(key);
        }

        [Test]
        public async Task StringDeleteAsync_Should_Delete_Key_From_Redis()
        {
            // Arrange
            var redisDbProvider = Substitute.For<IRedisDbProvider>();
            var cacheHandler = new RedisCacheHandler(redisDbProvider);
            var key = "testKey";

            // Act
            await cacheHandler.StringDeleteAsync(key);

            // Assert
            await redisDbProvider.database.Received(1).StringGetDeleteAsync(key);
        }

        [Test]
        public async Task HashSetAsync_Should_Set_Value_In_Redis()
        {
            // Arrange
            var redisDbProvider = Substitute.For<IRedisDbProvider>();
            var cacheHandler = new RedisCacheHandler(redisDbProvider);
            var key = "testKey";
            var hashField = "testHashField";
            var value = "testValue";

            // Act
            await cacheHandler.HashSetAsync(key, hashField, value);

            // Assert
            await redisDbProvider.database.Received(1).HashSetAsync(key, hashField, value);
        }

        [Test]
        public async Task HashSetAsync_Should_Set_Multiple_Values_In_Redis()
        {
            // Arrange
            var redisDbProvider = Substitute.For<IRedisDbProvider>();
            var cacheHandler = new RedisCacheHandler(redisDbProvider);
            var key = "testKey";
            var values = new List<KeyValuePair<string, string?>>()
            {
                new KeyValuePair<string, string?>("testHashField1", "testValue1"),
                new KeyValuePair<string, string?>("testHashField2", "testValue2"),
                new KeyValuePair<string, string?>("testHashField3", "testValue3"),
            };

            // Act
            await cacheHandler.HashSetAsync(key, values);

            // Assert
            await redisDbProvider.database.Received(1).HashSetAsync(key, Arg.Any<HashEntry[]>());
        }

        [Test]
        public async Task HashGetAsync_GetValueFromHashSet_ReturnsValue()
        {
            // Arrange
            var redisDbProvider = Substitute.For<IRedisDbProvider>();
            var cacheHandler = new RedisCacheHandler(redisDbProvider);
            var key = "testKey";
            var hashField = "testHashField";
            var value = "testValue";
            var hashEntries = new List<HashEntry>()
            {
                new HashEntry(hashField, value)
            };
            redisDbProvider.database.HashGetAsync(key, hashField).Returns(value);

            // Act
            var result = await cacheHandler.HashGetAsync(key, hashField);

            // Assert
            await redisDbProvider.database.Received(1).HashGetAsync(key, hashField);
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public async Task HashGetAsync_GetValueFromHashSet_ReturnsNull()
        {
            // Arrange
            var redisDbProvider = Substitute.For<IRedisDbProvider>();
            var cacheHandler = new RedisCacheHandler(redisDbProvider);
            var key = "testKey";
            var hashField = "testHashField";
            
            redisDbProvider.database.HashGetAsync(key, hashField).Returns((string?)null);

            // Act
            var result = await cacheHandler.HashGetAsync(key, hashField);

            // Assert
            await redisDbProvider.database.Received(1).HashGetAsync(key, hashField);
            Assert.IsNull(result);
        }
    }
}