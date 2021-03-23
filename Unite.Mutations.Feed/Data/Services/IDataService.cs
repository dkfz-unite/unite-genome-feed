﻿using System.Collections.Generic;

namespace Unite.Mutations.Feed.Data.Services
{
    public interface IDataService<TModel>
        where TModel : class
    {
        void SaveData(TModel model);
        void SaveData(IEnumerable<TModel> models);
    }

    public interface IDataService<TModel, TAudit>
        where TModel : class
        where TAudit : class
    {
        void SaveData(TModel model, out TAudit audit);
        void SaveData(IEnumerable<TModel> models, out TAudit audit);
    }
}