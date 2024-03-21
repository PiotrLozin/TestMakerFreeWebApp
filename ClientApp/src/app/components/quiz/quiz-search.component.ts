import { Component, Input, Inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { Quiz } from "../../interfaces/quiz";

@Component({
  selector: "quiz-search",
  templateUrl: './quiz-search.component.html',
  styleUrls: ['./quiz-search.component.css']
})

export class QuizSearchComponent {
  @Input() class: string | undefined;
  @Input() placeholder: string | undefined;
}
