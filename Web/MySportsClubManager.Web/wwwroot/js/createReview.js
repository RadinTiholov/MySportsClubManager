let stars = document.getElementsByClassName("input-stars");
let button = document.getElementById("review-button");
let checkedStar = 1;

for (let i = 0; i < stars.length; i++) {
    stars[i].onclick = () => {
        checkedStar = i + 1;
        for (let j = i; j >= 0; j--) {
            stars[j].classList.add("checked");
        }
        for (let k = i + 1; k < stars.length; k++) {
            if (stars[k].classList.contains("checked")) {
                stars[k].classList.remove("checked");
            }
        }
    }
}
function onButtonClick(e, clubId) {
    e.preventDefault();
    let reviewText = document.getElementById("review-text").value;
    let reviewErrorElement = document.getElementById("review-error");
    if (reviewText.length < 20 || reviewText.length > 200) {
        //Show error message
        reviewErrorElement.textContent = "Text must be between 20 and 200 characters!"
    }
    else {
        fetch("/api/Review", {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': document.getElementById("RequestVerificationToken")
            },
            body: JSON.stringify({
                rating: checkedStar,
                reviewText: reviewText,
                clubId: clubId
            })
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }

                throw new Error("Something went wrong or you've already posted a review!");
            })
            .then((data) => {
                let reviewContainerElement = document.getElementById("review-container");
                let starsHtml = '';
                for (let i = 0; i < data.review.rating; i++) {
                    starsHtml += '<span class="fa fa-star star-in-review"></span>';
                }
                for (let i = data.review.rating; i < 5; i++) {
                    starsHtml += '<span class="fa-regular fa-star star-in-review"></span>';
                }
                let html = `<div class="col">
                                    <div class="card bg-warning" style="border: 0px">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-md-1">
                                                     <img class="profile-picture rounded-circle" src="${data.review.userProfilePic}" alt="profile picture" />
                                                </div>
                                                <div class="col">
                                                    <h3>${data.review.userName}</h3>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col">
                                                    ${starsHtml}
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col">
                                                    <p>${data.review.reviewText}</p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>`;
                reviewContainerElement.innerHTML += html;
                console.log(data);
            })
            .catch(err => { reviewErrorElement.textContent = err })
    }
}
