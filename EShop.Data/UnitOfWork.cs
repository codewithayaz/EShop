using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Data.Entities;
using EShop.Data.Interfaces;

namespace EShop.Data
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private IMapper _mapper;
        private ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public ApplicationDbContext ApplicationDbContext => _context;
        private IGenericRepository<UserAudit> userAuditRepo;
        public IGenericRepository<UserAudit> UserAuditRepository => userAuditRepo ?? new GenericRepository<UserAudit>(_context, _mapper);

        private IGenericRepository<Category> categoryRepo;
        public IGenericRepository<Category> CategoryRepository => categoryRepo ?? new GenericRepository<Category>(_context, _mapper);

        private IGenericRepository<Product> productRepo;
        public IGenericRepository<Product> ProductRepository => productRepo ?? new GenericRepository<Product>(_context, _mapper);

        private IGenericRepository<Promotion> promotionRepo;
        public IGenericRepository<Promotion> PromotionRepository => promotionRepo ?? new GenericRepository<Promotion>(_context, _mapper);

        private IGenericRepository<ProductPromotion> productPromotionRepo;
        public IGenericRepository<ProductPromotion> ProductPromotionRepository => productPromotionRepo ?? new GenericRepository<ProductPromotion>(_context, _mapper);

        private IGenericRepository<CartItem> cartItemRepo;
        public IGenericRepository<CartItem> CartItemRepository => cartItemRepo ?? new GenericRepository<CartItem>(_context, _mapper);

        private IGenericRepository<Invoice> invoiceRepo;
        public IGenericRepository<Invoice> InvoiceRepository => invoiceRepo ?? new GenericRepository<Invoice>(_context, _mapper);

        private IGenericRepository<InvoiceDetail> invoiceDetailRepo;
        public IGenericRepository<InvoiceDetail> InvoiceDetailRepository => invoiceDetailRepo ?? new GenericRepository<InvoiceDetail>(_context, _mapper);

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
