<template>
  <Breadcrumbs :paths="breadcrumbs" />
  <section class="container">
    <h2 class="text-center">Мои срещи</h2><br />
    <div v-if="this.loading == true">Зареждане...</div>
    <div v-else>
      <div class="container">
        <Heading heading="Запазени срещи" />
        <br />
        <div class="row text-center">
          <div class="col"><strong>Заглавие</strong></div>
          <div class="col"><strong>Започва на</strong></div>
          <div class="col"><strong>Жени</strong></div>
          <div class="col"><strong>Мъже</strong></div>
          <div class="col"><strong>Цена</strong></div>
          <div class="col"><strong>Етнос</strong></div>
          <div class="col"><strong></strong></div>
        </div>
        <br />
        <div v-if="!this.savedSpots || this.savedSpots.length === 0">
          <p class="text-center">
            Нямате запазени срещи. Можете да го направите от <router-link to="/">началната страница</router-link>
          </p>
        </div>
        <div class="row text-center" v-else v-for="(spot, index) in this.savedSpots" :key="index">
          cards
        </div>
        <br />
        <Heading heading="Купени срещи" />
        <br />
        <div v-if="!this.orders || this.orders.length === 0">
          <p class="text-center">
            Не сте се записали за срещи. Можете да го направите от <router-link to="/">началната страница</router-link>
          </p>
        </div>
        <div v-else v-for="(order, index) in this.orders" :key="index">
          cards
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
import Breadcrumbs from '../components/Breadcrumbs.vue';
import Heading from '../components/Heading.vue';

export default {
    data() {
        return {
            orders: null,
            loading: false,
            breadcrumbs: [
              {name: '/', title: 'Начало'},
              {name: 'orders', title: 'Мои срещи'}
            ]
        };
    },
    async created() {
        this.loading = true;
        const authStore = useAuthStore();
        this.orders = await fetchWrapper.get('Order/GetByUserId?userId=' + authStore.user.id);
        this.savedSpots = await fetchWrapper.get('Dates/GetSavedSpots?userId=' + authStore.user.id);
        this.loading = false;
    },
    methods: {
        async cancel(order) {
            await fetchWrapper.put('Order/Cancel?id=' + order.id)
                .then((e) => {
                order.status = 'Отказана';
            });
        }
    },
    components: { Breadcrumbs, Heading }
}
</script>

<style scoped></style>