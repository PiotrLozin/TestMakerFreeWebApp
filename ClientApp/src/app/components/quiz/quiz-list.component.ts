import { HttpClient } from "@angular/common/http";
import { Component, Inject, Input, OnInit } from "@angular/core";
import { Quiz } from "../../interfaces/quiz";
import { Router } from "@angular/router";

@Component({
  selector: "quiz-list",
  templateUrl: './quiz-list.component.html',
  styleUrls: ['./quiz-list.component.css']
})

export class QuizListComponent implements OnInit {
  @Input() class: string | undefined;
  title: string | undefined;
  selectedQuiz: Quiz | undefined;
  quizzes: Quiz[] | undefined;
  baseUrl: string;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private router: Router  ) {
    this.baseUrl = "https://localhost:7136/api/quiz/";
  }

  ngOnInit() {
    console.log("QuizListComponent was created with class: " + this.class);
    var url = this.baseUrl;

    switch (this.class) {
      case "latest":
      default:
        this.title = "Newest quizes";
        url += "Latest/";
        break;
      case "byTitle":
        this.title = "Quizzes alphabetically";
        url += "ByTitle/";
        break;
      case "byTitle":
        this.title = "Quizzes randomly";
        url += "Random/";
        break;
    }

    this.http.get<Quiz[]>(url).subscribe(result => {
      this.quizzes = result;
    }, error => console.error(error));
  }

  onSelect(quiz: Quiz) {
    this.selectedQuiz = quiz;
    console.log("Selected was quiz with identificator: "
      + this.selectedQuiz.Id);
    this.router.navigate(["quiz", this.selectedQuiz.Id]);
  }
}


