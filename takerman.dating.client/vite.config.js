import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
// import 'owl.carousel/dist/assets/owl.carousel.css';
// import 'owl.carousel';
// import 'easing';

const baseFolder =
    process.env.APPDATA !== undefined && process.env.APPDATA !== ''
        ? `${process.env.APPDATA}/ASP.NET/https`
        : `${process.env.HOME}/.aspnet/https`;

const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
const certificateName = certificateArg ? certificateArg.groups.value : "takerman.dating.client";

if (!certificateName) {
    console.error('Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.')
    process.exit(-1);
}

const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '^/User': {
                target: 'https://127.0.0.1:7203/',
                secure: false
            },
            '^/Options': {
                target: 'https://127.0.0.1:7203/',
                secure: false
            },
            '^/Order': {
                target: 'https://127.0.0.1:7203/',
                secure: false
            },
            '^/Dates': {
                target: 'https://127.0.0.1:7203/',
                secure: false
            },
            '^/Notification': {
                target: 'https://127.0.0.1:7203/',
                secure: false
            },
            '^/Admin': {
                target: 'https://127.0.0.1:7203/',
                secure: false
            },
            '^/Cdn': {
                target: 'https://127.0.0.1:7203/',
                secure: false
            },
            '^/chatHub': {
                target: 'https://127.0.0.1:7203/',
                secure: false,
                ws: true
            },
        },
        port: 5175,
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        }
    }
})
