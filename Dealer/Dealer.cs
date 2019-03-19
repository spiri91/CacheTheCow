using Services;
using System;
using System.Collections.Generic;
using Obligations;

public class Dealer
{
    private static readonly Dictionary<string, Func<IService>> Instances = new Dictionary<string, Func<IService>>()
    {
        { nameof(IMakeHttpRequest), () =>  new HttpService() },
        { nameof(IHandleResponse), () =>  new HandleResponseService() }
    };

    public dynamic GiveMeA<T>() where T : class
    {
        if (Instances.ContainsKey(typeof(T).Name)) return Instances[typeof(T).Name]();

        throw new ArgumentException();
    }
}
