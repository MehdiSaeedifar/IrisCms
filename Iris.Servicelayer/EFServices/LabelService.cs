using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model.AdminModel;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.Caching;

namespace Iris.Servicelayer.EFServices
{
    public class LabelService : ILabelService
    {
        private readonly IDbSet<Label> _labels;

        public LabelService(IUnitOfWork uow)
        {
            _labels = uow.Set<Label>();
        }

        public void Add(Label label)
        {
            _labels.Add(label);
        }

        public void Remove(int id)
        {
            _labels.Remove(_labels.Find(id));
        }

        public void Update(Label label)
        {
            Label selectedLabel = _labels.Find(label.Id);
            selectedLabel.Name = label.Name;
            selectedLabel.Description = label.Description;
        }

        public Label GetLabel(Func<Label, bool> expression)
        {
            return _labels.Where(expression).FirstOrDefault();
        }

        public Label GetLabel(int id)
        {
            return _labels.Find(id);
        }

        public IList<Label> GetAll()
        {
            return _labels.AsNoTracking().ToList();
        }

        IList<Label> ILabelService.GetAll(int page, int count)
        {
            return _labels.AsNoTracking().Include(label => label.Posts).OrderByDescending(label => label.Id)
                .Skip(page * count).Take(count).ToList();
        }

        public IList<Label> GetAll(int labelId, int page, int count)
        {
            return
                _labels.AsNoTracking()
                    .Where(label => label.Id == labelId)
                    .Include(label => label.Posts)
                    .OrderByDescending(label => label.Id)
                    .Skip(page * count).Take(count).ToList();
        }


        public IList<Label> GetLabelsById(params int[] labelsId)
        {
            var lstLables = new List<Label>();
            foreach (int id in labelsId)
            {
                lstLables.Add(GetLabel(id));
            }
            return lstLables.ToList();
        }


        public int Count
        {
            get { return _labels.Count(); }
        }


        public IList<LabelDataTableModel> GetAll(string term, int page, int count, Order order, LabelOrderBy orderBy,
            LabelSearchBy searchBy)
        {
            IQueryable<Label> selectedLabels = _labels.Include(label => label.Posts).AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                switch (searchBy)
                {
                    case LabelSearchBy.Name:
                        selectedLabels = selectedLabels.Where(label => label.Name.Contains(term)).AsQueryable();
                        break;
                    case LabelSearchBy.Description:
                        selectedLabels = selectedLabels.Where(label => label.Description.Contains(term)).AsQueryable();
                        break;
                    default:
                        break;
                }
            }


            if (order == Order.Asscending)
            {
                switch (orderBy)
                {
                    case LabelOrderBy.Id:
                        selectedLabels = selectedLabels.OrderBy(label => label.Id).AsQueryable();
                        break;
                    case LabelOrderBy.Name:
                        selectedLabels = selectedLabels.OrderBy(label => label.Name).AsQueryable();
                        break;
                    case LabelOrderBy.Description:
                        selectedLabels = selectedLabels.OrderBy(label => label.Description).AsQueryable();
                        break;
                    case LabelOrderBy.PostCount:
                        selectedLabels = selectedLabels.OrderBy(label => label.Posts.Count).AsQueryable();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (orderBy)
                {
                    case LabelOrderBy.Id:
                        selectedLabels = selectedLabels.OrderByDescending(label => label.Id).AsQueryable();
                        break;
                    case LabelOrderBy.Name:
                        selectedLabels = selectedLabels.OrderByDescending(label => label.Name).AsQueryable();
                        break;
                    case LabelOrderBy.Description:
                        selectedLabels = selectedLabels.OrderByDescending(label => label.Description).AsQueryable();
                        break;
                    case LabelOrderBy.PostCount:
                        selectedLabels = selectedLabels.OrderByDescending(label => label.Posts.Count).AsQueryable();
                        break;
                    default:
                        break;
                }
            }
            return
                selectedLabels.Skip(page * count)
                    .Take(count)
                    .Select(
                        label =>
                            new LabelDataTableModel
                            {
                                Id = label.Id,
                                Name = label.Name,
                                Description = label.Description,
                                PostCount = label.Posts.Count
                            })
                    .ToList();
        }

        [CacheMethod(SecondsToCache = 120)]
        public IList<Label> GetLabels(Func<Label, bool> expression)
        {
            return _labels.Where(expression).ToList();
        }


        bool ILabelService.IsExist(string name)
        {
            return _labels.Where(label => label.Name.Equals(name)).Any();
        }
    }
}