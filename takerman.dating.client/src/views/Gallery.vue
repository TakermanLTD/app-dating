<template>
    <div class="container">
        <img :src="this.avatar == null || this.avatar == '' ? 'defaultAvatar.png' : this.avatar" class="img" width="150"
            height="150" />
        <button class="btn btn-primary" @click="this.unsetAvatar">Махни профилната</button> <br />
        <input id="fileUpload" @change="this.upload" type="file" multiple class="btn btn-primary" />
        <div v-if="this.loading === true">Зарежда...</div>
        <div class="pull-left" v-if="this.pictures && this.pictures.length > 0"
            v-for="(picture, pictureKey) in  this.pictures " :key="pictureKey">
            <img :src="picture.data" class="img" :title="'Uploaded on' + picture.uploadOn" width="150" height="150" />
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
            fetch('Options/GetAvatar?userId=' + this.userId)
                .then((response) => response.text())
                .then((text) => {
                    this.avatar = text;
                });
        },
        async getPictures() {
            this.loading = true;
            let result = await fetchWrapper.get('Options/GetUserPictures?id=' + this.userId);
            this.loading = false;
            return result;
        },
        async upload(e) {
            try {
                this.loading = true;
                let result = [];
                let files = e.currentTarget.files;

                for (let i = 0; i < files.length; i++) {
                    let file = files[i];
                    let encodedFile = await this.encodeFile(file);
                    result.push({
                        userId: this.userId,
                        data: encodedFile
                    });
                }

                await fetchWrapper.post('Options/UploadUserPictures', result)
                    .then((response) => {
                        this.loading = false;
                        for (let i = 0; i < response.length; i++) {
                            const picture = response[i];
                            this.pictures[this.pictures.length] = picture;
                        }
                        this.loading = false;
                        document.getElementById('fileUpload').value = null;
                    });
            } catch (error) {
                this.loading = false;
                console.log(error);
            }
        },
        async setAvatar(id) {
            await fetchWrapper.put('Options/SetAvatar?userId=' + this.userId + '&id=' + id)
                .then((result) => {
                    fetch('Options/GetAvatar?userId=' + this.userId)
                        .then((response) => response.text())
                        .then((text) => {
                            this.avatar = text;
                        });
                });
        },
        async remove(id) {
            await fetchWrapper.delete('Options/DeletePicture?id=' + id)
                .then((response) => {
                    for (let i = 0; i < this.pictures.length; i++) {
                        const picture = this.pictures[i];
                        if (picture.id === id) {
                            if (this.avatar == picture.data) {
                                this.avatar = null;
                                this.setAvatar(0);
                            }
                            this.pictures.splice(i, 1);
                        }
                    }
                });
        },
        async unsetAvatar() {
            await fetchWrapper.put('Options/UnsetAvatar?userId=' + this.userId)
                .then((result) => {
                    this.avatar = null;
                });
        },
        encodeFile(file) {
            return new Promise((resolve) => {
                var fr = new FileReader();
                fr.readAsDataURL(file);
                fr.onload = () => {
                    resolve(fr.result);
                };
                fr.onerror = function (error) {
                    console.log('Error: ', error);
                }
            });
        }
    }
};
</script>