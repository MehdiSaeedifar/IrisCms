using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Servicelayer.Interfaces;

namespace Iris.Servicelayer.EFServices
{
    public class DownloadLinkService : IDownloadLinkService
    {
        private readonly IDbSet<DownloadLink> _downloadLinks;
        private IUnitOfWork _uow;

        public DownloadLinkService(IUnitOfWork uow)
        {
            _uow = uow;
            _downloadLinks = _uow.Set<DownloadLink>();
        }

        public void RemoveByPostId(int id)
        {
            List<DownloadLink> selectedDownloadLinks = _downloadLinks.Where(x => x.Post.Id == id).ToList();
            foreach (DownloadLink link in selectedDownloadLinks)
            {
                _downloadLinks.Remove(link);
            }
        }
    }
}