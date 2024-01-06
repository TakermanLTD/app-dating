<template>
  <section id="breadcrumbs" class="breadcrumbs">
    <div class="container">
      <ol>
        <li><router-link to="/">Начало</router-link></li>
        <li>Мои срещи</li>
      </ol>
    </div>
  </section>
  <section class="container">
    <h2 class="text-center">Мои срещи</h2><br />
    <div v-if="this.loading == true">Зареждане...</div>
    <div v-else>
      <div class="row">
        <div class="col text-left"><strong>Номер</strong></div>
        <div class="col text-center"><strong>Цена</strong></div>
        <div class="col text-center"><strong>Статус</strong></div>
        <div class="col-3 text-center"><strong></strong></div>
      </div>
      <br />
      <div v-if="!this.orders || this.orders.length === 0">
        <p class="text-center">
          Не сте се записали за срещи. Можете да го направите от <router-link to="/">началната страница</router-link>
        </p>
      </div>
      <div v-else v-for="(order, index) in this.orders" :key="index">
        <div class="row">
          <div class="col text-left">{{ order.id }}</div>
          <div class="col text-center">{{ order.total?.toFixed(2) }} {{ order.currency }}</div>
          <div class="col text-center">{{ order.status }}</div>
          <div class="col-3 text-center">
            <table v-if="order.refundable">
              <tr>
                <td>
                  <button class="btn btn-secondary" @click="this.cancel(order)">Отказ</button>
                </td>
              </tr>
            </table>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="js">
import moment from 'moment';
import { fetchWrapper, router } from '@/helpers';
import { useAuthStore } from '@/stores';

export default {
  data() {
    return {
      moment: moment,
      orders: null,
      loading: false
    }
  },
  async created() {
    this.loading = true;
    const authStore = useAuthStore();
    this.orders = await fetchWrapper.get('Order/GetByUserId?userId=' + authStore.user.id);
    this.loading = false;
  },
  methods: {
    async cancel(order) {
      await fetchWrapper.put('Order/Cancel?id=' + order.id)
        .then((e) => {
          order.status = 'Отказана';
        });
    }
  }
}
</script>

<style scoped></style>