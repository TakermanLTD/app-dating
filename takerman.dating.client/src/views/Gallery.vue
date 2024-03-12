<template>
    <div class="container">
        <img :src="this.avatar" class="img" width="150" height="150" />
        <button class="btn btn-primary" @click="this.unsetAvatar">Махни профилната</button> <br />
        <input id="fileUpload" @change="this.upload" type="file" class="btn btn-primary" />
        <div v-if="this.loading === true">Зарежда...</div>
        <div class="pull-left" v-if="this.pictures && this.pictures.length > 0"
            v-for="(picture, pictureKey) in  this.pictures " :key="pictureKey">
            <img :src="picture.url" class="img" :title="'Uploaded on' + picture.uploadOn" width="150" height="150" />
            <button class="btn btn-danger" @click="remove(picture.id)">Изтрий</button>
            <button class="btn btn-primary" @click="setAvatar(picture.id)">Профилна</button>
        </div>
    </div>
</template>
<script>
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
export default {
    data() {
        return {
            loading: false,
            userId: null,
            avatar: null,
            pictures: []
        };
    },
    async mounted() {
        const authStore = useAuthStore();
        this.userId = authStore.user.id;
        this.pictures = await this.getPictures();
        this.loadAvatar();
    },
    methods: {
        async loadAvatar() {
            let result = await fetch('Cdn/GetAvatar?userId=' + this.userId);
            this.avatar = await result.text();
        },
        async getPictures() {
            this.loading = true;
            let result = await fetchWrapper.get('Cdn/GetUserPictures?userId=' + this.userId);
            this.loading = false;
            return result;
        },
        async upload(e) {
            this.loading = true;
            let file = e.currentTarget.files[0];

            var model = {
                userId: this.userId,
                picture: []
            };
            var reader = new FileReader();
            reader.readAsArrayBuffer(file);
            reader.onloadend = async function (evt) {
                if (evt.target.readyState == FileReader.DONE) {
                    model.picture = new Uint8Array(evt.target.result);

                    await fetchWrapper.post('Cdn/UploadUserPictures', model).then((response) => {
                        this.loading = false;
                        for (let i = 0; i < response.length; i++) {
                            const picture = response[i];
                            this.pictures[this.pictures.length] = picture;
                        }
                        this.loading = false;
                        document.getElementById('fileUpload').value = null;
                    });
                }
            }
        },
        async setAvatar(url) {
            this.avatar = url;
            await fetch('Cdn/SetAvatar?userId=' + this.userId + '&url=' + url);
        },
        async remove(id) {
            await fetchWrapper.delete('Cdn/DeletePicture?id=' + id)
                .then((response) => {
                    for (let i = 0; i < this.pictures.length; i++) {
                        const picture = this.pictures[i];
                        if (picture.id === id) {
                            if (this.avatar == picture.url) {
                                this.avatar = null;
                                this.setAvatar(0);
                            }
                            this.pictures.splice(i, 1);
                        }
                    }
                });
        },
        async unsetAvatar() {
            let result = await fetch('Cdn/UnsetAvatar?userId=' + this.userId);
            return await result.text();
        },
        encodeFile(file) {
        }
    }
};
</script>