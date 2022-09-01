﻿using CafeteriaOrders.data;
using CafeteriaOrders.logic;
using CafeteriaOrders.logic.DtosModels;
using CafeteriaOrders.logic.DtosModels.Carts;
using CafeteriaOrders.logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CafeteriaOrders.Service.CartServices
{
    public class CartServices : ICartService
    {
        Context context;
        UnitOfWork uof;
        public CartServices(Context context)
        {
            uof = new UnitOfWork(context);
        }
        
        public async Task<AddCartDtos> Add(AddCartDtos model)
        {
            var cart = uof.carts.add(model);
            uof.Commit();
            return model;
        }

                                                                                                                                                                                                                                                                
        public  async Task<Cart> Delete(int id)
        {
            var meal = uof.carts.remove(id);
            uof.Commit();
            return meal;
        }

        public async Task<GetCartDtos> Details(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Cart> Edit(GetCartDtos model)
        {
            return uof.carts.edit(model);
        }

        public async Task<IEnumerable<GetCartDtos>> Get()
        {
            return uof.carts.get();
        }

        public async Task<IEnumerable<CartItemDtos>> GetCartItems()
        {
            throw new NotImplementedException();
        }

        public async Task<decimal> GetTotalPrice()
        {
            throw new NotImplementedException();
        }
        public  decimal checkValidItem(CartItem model) {
            /*
             1-  get meal details by id
             2- check quantity 
             */
            var meal = uof.meal.details(model.mealId);
            decimal totalPrice =0;
            if(meal.numberofUnits == model.quantity)
            {
                totalPrice = model.quantity * meal.price;
                return totalPrice;
            }

            return totalPrice;
        }

        public async Task<ServiceResponse<GetCartDtos>> checkout(List<CartItem> model)
        {
            var service = new ServiceResponse<GetCartDtos>();
            string massage= "";
             var cart = new GetCartDtos();
            decimal totalprice = 0;
            foreach (var item in model)
            {
                decimal price = checkValidItem(item);
                if (price.Equals(0.0))
                {
                    cart.cartItems.Add(item);
                   // listValid.Add(item);
                    totalprice += price;
                }
                else
                {
                    var meal = uof.meal.details(item.mealId);
                    massage += meal.name + " and ";
                }
            }
            cart.totalPrice = totalprice;
            service.Data = cart;
            service.Message = massage+"this items is not vaild";

            return service;
        }
    }
}
