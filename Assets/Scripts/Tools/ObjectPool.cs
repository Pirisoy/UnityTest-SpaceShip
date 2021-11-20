using System.Collections.Generic;

public class ObjectPool<T> where T : IPoolObject
{
    private Queue<T> pool;

    public ObjectPool()
    {
        pool = new Queue<T>();
    }

    public void Add(T obj)
    {
        pool.Enqueue(obj);
    }
    public T Get()
    {
        T obj = pool.Dequeue();
        obj.OnSpawn();

        return obj;
    }
    public bool HasItem()
    {
        return pool.Count > 0;
    }
}
public interface IPoolObject
{
    void OnSpawn();
}