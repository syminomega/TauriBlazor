// vite.config.js
import { resolve } from 'path'
import { defineConfig } from 'vite'
export default defineConfig({
    build: {
        lib: {
            entry: {
                'tauri-api': resolve(__dirname, 'src/main.ts'),
                'tauri-utils': resolve(__dirname, 'src/utils.ts'),
            },
            // fileName: (format, entryName) => `${entryName}.js`,
        },
        rollupOptions: {
            // 确保外部化处理那些你不想打包进库的依赖
            // external: ['vue'],
            output: {
                // 在 UMD 构建模式下为这些外部化的依赖提供一个全局变量
                // globals: {
                //     vue: 'Vue',
                // },
                format: 'esm',
            },
        },
    }
})