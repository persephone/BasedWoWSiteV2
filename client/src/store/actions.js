import axios from 'axios';
import config from "../config";

const API_STORE = config.API.STORE;
const API_AUTH  = config.API.AUTH;

import {
  ADD_PRODUCT,
  ADD_PRODUCT_SUCCESS,
  PRODUCT_BY_ID,
  PRODUCT_BY_ID_SUCCESS,
  UPDATE_PRODUCT,
  UPDATE_PRODUCT_SUCCESS,
  REMOVE_PRODUCT,
  REMOVE_PRODUCT_SUCCESS,
  ALL_PRODUCTS,
  ALL_PRODUCTS_SUCCESS,
  ALL_MANUFACTURERS,
  ALL_MANUFACTURERS_SUCCESS,
  AUTH_REQUEST,
  AUTH_SUCCESS,
  AUTH_ERROR,
  AUTH_LOGOUT
} from "./mutation-types";

export const productActions = {
  allProducts({ commit }) {
    commit(ALL_PRODUCTS);
    axios.get(`${API_STORE}/products`).then(response => {
      commit(ALL_PRODUCTS_SUCCESS, response.data);
    });
  },
  productById({ commit }, payload) {
    commit(PRODUCT_BY_ID);
    axios.get(`${API_STORE}/products/${payload}`).then(response => {
      console.log(payload, response.data);
      commit(PRODUCT_BY_ID_SUCCESS, response.data);
    });
  },
  addProduct({ commit }, payload) {
    commit(ADD_PRODUCT);
    axios.post(`${API_STORE}/products`, payload).then(response => {
      commit(ADD_PRODUCT_SUCCESS, response.data);
    });
  },
  updateProduct({ commit }, payload) {
    commit(UPDATE_PRODUCT);
    axios.put(`${API_STORE}/products/${payload._id}`, payload).then(response => {
      commit(UPDATE_PRODUCT_SUCCESS, response.data);
    });
  },
  removeProduct({ commit }, payload) {
    commit(REMOVE_PRODUCT);
    axios.delete(`${API_STORE}/products/${payload}`, payload).then(response => {
      console.debug("response", response.data);
      commit(REMOVE_PRODUCT_SUCCESS, response.data);
    });
  }
};

export const manufacturerActions = {
  allManufacturers({ commit }) {
    commit(ALL_MANUFACTURERS);
    axios.get(`${API_STORE}/manufacturers`).then(response => {
      commit(ALL_MANUFACTURERS_SUCCESS, response.data);
    });
  }
};

export const authActions = {
    async login({ commit }, loginModel) {
      commit(AUTH_REQUEST);
      try {
          const data = await axios.post(`${API_AUTH}/login`, loginModel);
          const { token, user } = data.data;

          console.log("auth success")
          localStorage.setItem("token", token);
          localStorage.setItem("user", JSON.stringify(user));

          axios.defaults.headers.common.Authorization = token;

          commit(AUTH_SUCCESS, token, user);
          return true;
      } catch (e) {
          commit(AUTH_ERROR);
          console.log("auth error" + e.message)
          localStorage.removeItem("token");
          localStorage.removeItem("user");
          return false;
      }
  },
  async logout({ commit }) {
      commit(AUTH_LOGOUT);
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      delete axios.defaults.headers.common.Authorization;
  }
}