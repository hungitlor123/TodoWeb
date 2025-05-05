using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using TodoWeb.Appllication.Services.CacheService;

namespace TodoWeb.Appllication.ActionFilters
{
    public class CacheFilter : ActionFilterAttribute
    {
        private readonly int _duration;
        private readonly IMemoryCache _cache;
        // private readonly ICacheService _cacheService;
        private string key;

        public CacheFilter( 
            // ICacheService cacheService, 
            IMemoryCache cache,int duration)
        {
            _duration = duration;
            // _cacheService = cacheService;
            _cache = cache;
        }

        public override void OnActionExecuting(ActionExecutingContext context)

        {
             key = context.HttpContext.Request.Path.ToString();
             
             var cacheData = _cache.Get(key);
             
             if (cacheData != null)
             {
                 context.Result = new ObjectResult(cacheData);
             }

             // if (_cache.TryGetValue(key, out var cachedValue))
             // {
             //     context.Result = new ObjectResult(cachedValue);
             //     return;
             // }
            // var cacheData = _cacheService.Get("key");
            // if (cacheData != null)
            // {
            //     context.Result = new ObjectResult(cacheData.Value);
            // }
            
            
            
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var data = context.Result as ObjectResult;

            if (data != null)
            {
                
                // _cacheService.Set(key, data.Value, _duration);
                
                _cache.Set(key, data.Value, TimeSpan.FromSeconds(_duration));
            }
        }

        
    }
}
