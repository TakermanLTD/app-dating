<template>
    <hgroup>
        <h2 class="text-center">Admin</h2>
    </hgroup>
    <div>
        <hgroup>
            <h3>Dates</h3>
        </hgroup>
        <table v-if="this.dates" class="table table-striped table-bordered">
            <tr>
                <th>ID</th>
                <th>title</th>
                <th>starts on</th>
                <th>status</th>
                <th>min men</th>
                <th>men</th>
                <th>min women</th>
                <th>women</th>
                <th>min ages</th>
                <th>max ages</th>
                <th>price</th>
                <th>ethnicity</th>
                <th>link</th>
                <th>action</th>
            </tr>
            <tr v-for="(date, dateKey) in this.dates" :key="dateKey">
                <td>{{ date.id }}</td>
                <td> <input type="text" :v-model="date.title" class="form-control" :value="date.title" /></td>
                <td> <input type="text" :v-model="date.startsOn" class="form-control" :value="moment(new Date(date.startsOn)).format('DD MMM, HH:mm')" style="width: 140px;" /></td>
                <td>
                    <select class="form-control" :value="date.status">
                        <option v-for="(status, statusKey) in this.statuses" :value="status.key" :key="statusKey" @click="date.status = status.key">{{ status.value }}</option>
                    </select>
                </td>
                <td> <input type="text" style="width: 80px;" :v-model="date.minMen" class="form-control" :value="date.minMen" /></td>
                <td> <input type="text" style="width: 80px;" :v-model="date.menCount" class="form-control" :value="date.menCount" /></td>
                <td> <input type="text" style="width: 80px;" :v-model="date.minWomen" class="form-control" :value="date.minWomen" /></td>
                <td> <input type="text" style="width: 80px;" :v-model="date.womenCount" class="form-control" :value="date.womenCount" /></td>
                <td> <input type="text" style="width: 80px;" :v-model="date.minAges" class="form-control" :value="date.minAges" /></td>
                <td> <input type="text" style="width: 80px;" :v-model="date.maxAges" class="form-control" :value="date.maxAges" /></td>
                <td> <input type="text" style="width: 80px;" :v-model="date.price" class="form-control" :value="date.price" /></td>
                <td>
                    <select class="form-control" :value="date.ethnicity">
                        <option v-for="(ethnicity, ethnicityKey) in this.ethnicities" :value="ethnicity.key" :key="ethnicityKey" @click="date.ethnicity = ethnicity.key">{{ ethnicity.value }}</option>
                    </select>
                </td>
                <td> <input type="text" :v-model="date.videoLink" class="form-control" :value="date.videoLink" /></td>
                <td>
                    <button @click="saveDate(date)" class="btn btn-success">save</button>
                    <button @click="removeDate(date)" class="btn btn-danger">remove</button>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <hgroup>
            <h3>Users</h3>
        </hgroup>
        <table v-if="this.users" class="table table-striped table-bordered">
            <tr>
                <th>ID</th>
                <th>first name</th>
                <th>last name</th>
                <th>email</th>
                <th>country</th>
                <th>city</th>
                <th>phone</th>
                <th>facebook</th>
                <th>instagram</th>
            </tr>
            <tr v-for="(user, userKey) in this.users" :key="userKey">
                <td>{{ user.id }}</td>
                <td>{{ user.firstName }}</td>
                <td>{{ user.lastName }}</td>
                <td>{{ user.email }}</td>
                <td>{{ user.country }}</td>
                <td>{{ user.city }}</td>
                <td>{{ user.phone }}</td>
                <td>{{ user.facebook }}</td>
                <td>{{ user.instagram }}</td>
            </tr>
        </table>
    </div>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import moment from 'moment';

export default {
    data() {
        return {
            moment: moment,
            ethnicities: [],
            dateTypes: [],
            dates: [],
            users: [],
            statuses: []
        };
    },
    async mounted() {
        this.ethnicities = await fetchWrapper.get('Options/GetEthnicities');
        this.dateTypes = await fetchWrapper.get('Options/GetDateTypes');
        this.dates = await fetchWrapper.get('Dates/GetAll');
        this.users = await fetchWrapper.get('User/GetAll');
        this.statuses = await fetchWrapper.get('Options/GetDateStatuses');
    },
    component: {},
    methods: {
        async saveDate(date) {
            await fetchWrapper.post('Dates/Save', date);
        },
        async removeDate(date) {
            await fetchWrapper.delete('Dates/Delete', date.id);
        },
        async saveUser(date) {
            await fetchWrapper.post('Users/Save', date);
        },
        async removeUser(date) {
            await fetchWrapper.delete('Users/Delete', date);
        }
    },
    components: {}
}
</script>

<style scoped></style>