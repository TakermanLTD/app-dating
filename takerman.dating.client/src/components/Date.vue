<template>
  <breadcrumbs :paths="this.breadcrumbs" />
  <loader v-if="this.loading" />
  <div class="row text-center" v-else>
    <heading :heading="date?.title" />
    <card :date="date" />
    <div style="height: 100px;" class="col">
      <p class="center-text-vertically">
        {{ date.description }}
        <br />
        <br />
        <br />
        Бързите срещи са формализиран метод за запознанства лице-в-лице, насърчаващ хора да се запознаят с голям брой
        непознати. <br /> <br />
        Обикновено при бързите срещи се изисква предварителна регистрация. Мъжете и жените се разменят, за да се срещнат
        с всеки на редица кратки „срещи“, които продължават от 3 до 8 минути в зависимост от решение на организаторите,
        които осъществяват събитието. При повечето организатори жените остават седнали на отделни маси, а мъжете се
        редуват да сядат при тях. В края на всеки интервал от няколко минути организаторът издрънчава със звънец,
        почуква по чаша или дава сигнал със свирка, за да предупреди участниците да се преместят към следващата среща. В
        края на събитието участниците предават на организаторите списък с кого биха желали да се срещнат отново.
        Съществуват и варианти, при които участниците попълват резултатите си онлайн. Ако има съвпадения, участниците
        получават взаимно контактите на другия. По време на първоначалната среща не се разменят контакти с цел да се
        намали напрежението от директното одобряване или отказване на някой от кандидатите.
      </p>
    </div>
  </div>
</template>

<script lang="js">
import { fetchWrapper, router } from '@/helpers';
import { useAuthStore } from '@/stores';
import breadcrumbs from './breadcrumbs.vue';
import loader from './loader.vue';
import heading from './Heading.vue';
import card from './Card.vue';

export default {
  data() {
    return {
      id: 0,
      date: null,
      loading: false,
      breadcrumbs: [
        {
          name: '/',
          title: 'Начало'
        },
        {
          name: '#',
          title: 'Среща'
        }
      ]
    };
  },
  async created() {
    this.loading = true;
    let queryString = window.location.search;
    let urlParams = new URLSearchParams(queryString);
    if (urlParams.has('id')) {
      this.id = urlParams.get('id');
      this.date = await fetchWrapper.get('Dates/Get?id=' + this.id);
    }
    this.loading = false;
  },
  components: {
    loader,
    breadcrumbs,
    heading,
    card
  }
}
</script>

<style scoped>
.center-text-vertically {
  margin-top: 200px;
}
</style>