


const clothesData = {
    jeans: [
        { name: "Quần Jeans A", img: "/content/Images/tui.jpg" },
        { name: "Quần Jeans B", img: "jeans-b.jpg" },
        { name: "Quần Jeans A", img: "jeans-a.jpg" },
        { name: "Quần Jeans B", img: "jeans-b.jpg" }
    ],
    jacket: [
        { name: "Jacket X", img: "jacket-x.jpg" },
        { name: "Jacket Y", img: "jacket-y.jpg" }
    ],
    hat: [
        { name: "Mũ N", img: "hat-n.jpg" },
        { name: "Mũ O", img: "hat-o.jpg" }
    ]
};

function showClothes(type) {
    const grid = document.getElementById('ItemGrid');
    grid.innerHTML = ''; // Xóa nội dung cũ

    clothesData[type].forEach(item => {
        const col = document.createElement('div');
        col.classList.add('col-md-4'); // Mỗi sản phẩm chiếm 1/3 chiều rộng

        col.innerHTML = `
            <div class="product-card text-center">
                <div class="product-image">
                    <img src="${item.img}" alt="${item.name}" class="img-fluid">
                </div>
                <p class="product-name">${item.name}</p>
            </div>
        `;

        grid.appendChild(col);
    });
}
