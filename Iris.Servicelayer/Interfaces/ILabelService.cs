using System;
using System.Collections.Generic;
using Iris.DomainClasses.Entities;
using Iris.Model.AdminModel;
using Iris.Servicelayer.EFServices.Enums;

namespace Iris.Servicelayer.Interfaces
{
    public interface ILabelService
    {
        int Count { get; }
        void Add(Label label);
        void Remove(int id);
        void Update(Label label);
        Label GetLabel(Func<Label, bool> expression);
        Label GetLabel(int id);
        IList<Label> GetAll();
        IList<Label> GetAll(int page, int count);
        IList<Label> GetAll(int labelId, int page, int count);
        IList<Label> GetLabelsById(params int[] labelsId);

        IList<LabelDataTableModel> GetAll(string term, int page, int count,
            Order order, LabelOrderBy orderBy, LabelSearchBy searchBy);

        IList<Label> GetLabels(Func<Label, bool> expression);
        bool IsExist(string name);
    }
}