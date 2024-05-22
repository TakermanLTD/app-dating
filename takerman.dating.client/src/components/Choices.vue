<template>
    <div v-if="this.choices?.length > 0">
        <Heading :heading="$t('choices.title')" />
        <div id="myChoices" class="text-center">
            <div class="col" style="margin: 15px; width: 15rem; display: inline-block;"
                 v-for="(choice, index) in this.choices" :key="index">
                <img :src="choice.avatarUrl" class="img" width="150" height="150" />
                <div>
                    <span class="text-center">{{ choice.name }}</span> <br /><br />
                    <div>
                        Моя избор е: <br />
                        <label style="margin-right: 15px;" v-for="(radio, radioIndex) in choice.radios" :key="radioIndex"
                               class="form-check-label">
                            <input :disabled="this.date?.status == 'ResultsRevealed'" type="radio"
                                   @change="checkRadio(radio, choice.radios)" :value="radio.isChecked" class="form-check-input"
                                   :name="'choice_' + index" :checked="radio.isChecked == true">
                            {{ radio.label }}
                        </label>
                    </div>
                    <div v-if="this.date?.status == 'ResultsRevealed' && choice?.theirChoice != null">
                        <br />
                        <span>
                            Техния избор е: <br /><strong>{{ choice.theirChoice ? choice.theirChoice : 'Нищо' }}</strong>
                        </span> <br />
                        <div v-if="choice?.theirChoice === 'Yes' && choice.myChoice === 'Yes'">
                            Съвпадение! Можете да чатите.<br />
                            <router-link :to="'/matches'" tag="button" class="btn btn-info">Чат</router-link>
                            <router-link :to="'/user-profile?id=' + choice.voteForId" tag="button" class="btn btn-info">Профил</router-link>
                        </div>
                        <div v-else>
                            <button disabled class="btn btn-info">Чат</button>
                            <button disabled class="btn btn-info">Профил</button>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div>
                <button v-if="this.date?.status != 'ResultsRevealed'" class="btn btn-success"
                        @click="saveChoices">Запази</button>
                <div v-if="this.showStatus === true" class="alert alert-success" role="alert">
                    <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                    <span class="sr-only"></span> Запазихте избора си успешно
                </div>
            </div>
        </div>
        <br />
    </div>
    <div v-else>
        <p class="text-center">
            <strong>{{ $t('choices.noChoices') }}</strong>
        </p>
    </div>

</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
import Avatar from './Avatar.vue';
import Heading from './Heading.vue';

export default {
    props: {
        dateId: Number
    },
    data() {
        return {
            date: null,
            choices: [],
            showStatus: false
        }
    },
    computed: {
        isPast() {
            return new Date(this.date?.startsOn) < new Date();
        }
    },
    components: {
        Avatar, Heading
    },
    async mounted() {
        const authStore = useAuthStore();
        this.date = await fetchWrapper.get('Dates/Get?id=' + this.dateId);
        if (authStore.user) {
            this.choices = await fetchWrapper.get('Dates/GetChoices?userId=' + authStore.user.id + '&dateId=' + this.dateId);
            for (let i = 0; i < this.choices.length; i++) {
                const choice = this.choices[i];
                choice.avatarUrl = await this.getAvatarUrl(choice.voteForId);
            }
        }
    },
    methods: {
        async saveChoices() {
            const authStore = useAuthStore();
            await fetchWrapper.post('Dates/SaveChoices?userId=' + authStore.user.id + '&dateId=' + this.dateId, this.choices);
            this.showStatus = true;
        },
        checkRadio(radio, radios) {
            for (let i = 0; i < radios.length; i++) {
                const radio = radios[i];
                radio.isChecked = false;
            }

            radio.isChecked = true;
        },
        async getAvatarUrl(userId) {
            let result = await fetch('Cdn/GetAvatar?userId=' + userId);
            return await result.text();
        }
    }
}
</script>