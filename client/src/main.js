import Vue from "vue";
import App from "./App";
import router from "./router";
import VeeValidate from "vee-validate";
import BootstrapVue from "bootstrap-vue";
import store from "./store";
import Gravatar from "vue-gravatar";
import CKEditor from "@ckeditor/ckeditor5-vue";
import signalR from "./signalR/signalr";
import axios from "./helpers/axiosInterceptor";
import TextHighlight from "vue-text-highlight";
import datePicker from "vue-bootstrap-datetimepicker";

import CharacterComponent from "@/components/Admin/Characters/Views/CharacterComponent";

Vue.config.productionTip = false;

Vue.use(axios);
Vue.use(BootstrapVue);
Vue.use(signalR);
Vue.use(CKEditor);
Vue.use(Gravatar);
Vue.use(VeeValidate, { fieldsBagName: "veeFields" });
Vue.use(datePicker);

Vue.component("text-highlight", TextHighlight);
Vue.component("character-component", CharacterComponent);

store.dispatch("realms/FetchRealms").finally(() => {
  /* eslint-disable no-new */
  new Vue({
    el: "#app",
    store,
    router,
    components: { App },
    template: "<App/>",
    methods: {
      Toast(title, msg, variant = null) {
        this.$bvToast.toast(msg, {
          title: title,
          variant: variant,
          solid: true
        });
      }
    },
    created() {}
  });
});
