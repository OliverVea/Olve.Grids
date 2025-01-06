/** @type {import('tailwindcss').Config} */
export default {
    content: ['./index.html'],
    theme: {
        extend: {
            colors: {
                "transparent": "transparent",
                "mutedbrighttext": "#cccccc",
                "dimbrighttext": "#6e7270",
                "panelhover": "#3c3f41",
                "panel": "#2b2d30",
                "background": "#1e1f22",
            },
        },
    },
    plugins: [
        require('tailwind-scrollbar')],
}